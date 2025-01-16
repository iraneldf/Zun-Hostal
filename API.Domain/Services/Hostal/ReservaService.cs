using API.Data.Entidades.Hostal;
using API.Data.Entidades.Hostal.Enums;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Hostal;
using API.Domain.Validators.Hostal;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Domain.Services.Hostal;

public sealed class ReservaService : BasicService<Reserva, ReservaValidator>, IReservaService
{
    public ReservaService(IUnitOfWork<Reserva> repositorios, IHttpContextAccessor httpContext) : base(repositorios,
        httpContext)
    {
    }

    public override async Task<EntityEntry<Reserva>> Crear(Reserva reserva)
    {
        await ValidarAntesCrear(reserva);

        //calcular importe (cant días * 10)
        reserva.Importe = await CalcularImporte(reserva);
        reserva.FechaCreado = DateTime.Now;

        return await _repositorios.BasicRepository.AddAsync(EstablecerDatosAuditoria(reserva));
    }

    public override async Task<EntityEntry<Reserva>> Actualizar(Reserva reserva)
    {
        await ValidarAntesActualizar(reserva);

        reserva.Importe = await CalcularImporte(reserva);

        return _repositorios.BasicRepository.Update(EstablecerDatosAuditoria(reserva, false));
    }

    public override async Task ValidarAntesCrear(Reserva reserva)
    {
        await base.ValidarAntesCrear(reserva);

        // Validar que la habitación no esté ocupada en las fechas indicadas
        var habitacionOcupada = await _repositorios.Reserva.AnyAsync(h =>
                h.HabitacionId == reserva.HabitacionId && // Misma habitación
                !h.EstaCancelada && // Excluir reservas canceladas
                !(h.FechaEntrada < DateTime.Now && !h.LlegadaCliente) && // Excluir reservas pasadas sin llegada
                !(reserva.FechaEntrada > h.FechaSalida || reserva.FechaSalida < h.FechaEntrada) // Se solapan
        );

        if (habitacionOcupada)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación está ocupada en las fechas indicadas."
            };
        }

        // Validar que el cliente no tenga otra reserva en las mismas fechas
        var clienteConReserva = await _repositorios.Reserva.AnyAsync(r =>
            r.ClienteId == reserva.ClienteId &&
            !r.EstaCancelada && // Excluir la que este cancelada
            !(reserva.FechaEntrada > r.FechaSalida || reserva.FechaSalida < r.FechaEntrada));

        if (clienteConReserva)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Message = "El cliente ya tiene una reserva en las fechas indicadas."
            };
        }

        // Validar que las fechas sean posteriores a la fecha actual
        if (reserva.FechaSalida < DateTime.Today || reserva.FechaEntrada < DateTime.Today)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Las fechas deben ser posteriores a la actual"
            };

        // Validar que la fecha de entrada sea menor que la fecha de salida
        if (reserva.FechaEntrada >= reserva.FechaSalida)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de entrada no puede ser mayor o igual que la fecha de salida"
            };

        // Validar que el período de reserva sea de al menos 3 días
        var diferencia = reserva.FechaSalida - reserva.FechaEntrada;
        if (Math.Abs(diferencia.Days) < 2)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El periodo mínimo para realizar la reserva es de 3 días"
            };

        // Validar que la habitación exista
        var habitacion = await _repositorios.Habitacion.GetByIdAsync(reserva.HabitacionId) ??
                         throw new CustomException
                         {
                             Status = StatusCodes.Status400BadRequest,
                             Message = "La habitación especificada no existe"
                         };

        if (habitacion.Estado is EstadoHabitacion.FueraDeServicio)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación especificada está fuera de servicio en estos momentos"
            };
    }

    public override async Task ValidarAntesActualizar(Reserva reserva)
    {
        // Obtener reserva antigua
        var reservaAntigua = await _repositorios.Reserva.GetByIdAsync(reserva.Id) ?? throw new CustomException
        {
            Status = StatusCodes.Status404NotFound,
            Message = "Reserva no encontrada."
        };

        // No permimir si esta obsoleta
        if (reservaAntigua.FechaEntrada < DateTime.Today)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Esta reserva esta obsoleta"
            };
        }

        // No permitir si esta confirmada
        if (reservaAntigua.LlegadaCliente)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Esta reserva fue confirmada, no se puede editar."
            };
        }

        // Validar que la habitación no esté ocupada en las fechas indicadas (excluyendo la reserva actual)
        var habitacionOcupada = await _repositorios.Reserva.AnyAsync(h =>
                h.HabitacionId == reserva.HabitacionId && // Misma habitación
                h.Id != reserva.Id && // Excluir la reserva actual
                !h.EstaCancelada && // Excluir reservas canceladas
                !(h.FechaEntrada < DateTime.Now && !h.LlegadaCliente) && // Excluir reservas pasadas sin llegada
                !(reserva.FechaEntrada > h.FechaSalida || reserva.FechaSalida < h.FechaEntrada) // Se solapan
        );

        if (habitacionOcupada)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación está ocupada en las fechas indicadas."
            };
        }

        // Validar que el cliente no tenga otra reserva en las mismas fechas (excluyendo la reserva actual)
        var clienteConReserva = await _repositorios.Reserva.AnyAsync(r =>
            r.ClienteId == reserva.ClienteId &&
            !r.EstaCancelada && // Excluir la que este cancelada
            r.Id != reserva.Id && // Excluir la reserva actual
            !(reserva.FechaEntrada > r.FechaSalida || reserva.FechaSalida < r.FechaEntrada));

        if (clienteConReserva)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El cliente ya tiene una reserva en las fechas indicadas."
            };
        }

        // Validar que las fechas sean posteriores a la fecha actual
        if (reserva.FechaSalida < DateTime.Today || reserva.FechaEntrada < DateTime.Today)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Las fechas deben ser posteriores a la actual"
            };

        // Validar que la fecha de entrada sea menor que la fecha de salida
        if (reserva.FechaEntrada >= reserva.FechaSalida)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de entrada no puede ser mayor o igual que la fecha de salida"
            };

        // Validar que el período de reserva sea de al menos 3 días
        var diferencia = reserva.FechaSalida - reserva.FechaEntrada;
        if (Math.Abs(diferencia.Days) < 2)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El periodo mínimo para realizar la reserva es de 3 días"
            };

        // Validar que la habitación exista y esté disponible
        var habitacion = await _repositorios.Habitacion.GetByIdAsync(reserva.HabitacionId) ??
                         throw new CustomException
                         {
                             Status = StatusCodes.Status400BadRequest,
                             Message = "La habitación especificada no existe"
                         };

        if (habitacion.Estado == EstadoHabitacion.FueraDeServicio)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación especificada está fuera de servicio en estos momentos"
            };

        // Validar los datos insertados por el Rol
        await new ReservaValidator(_repositorios).ValidateAndThrowAsync(reserva);
    }

    public async Task<List<Reserva>> ObtenerReservasActivasPorFecha(DateTime fecha)
    {
        if (fecha == default)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha especificada no es válida."
            };

        var reservas = await _repositorios.Reserva.GetAllAsync(
            r => r.FechaEntrada <= fecha && r.FechaSalida >= fecha,
            query => query
                .Include(h => h.Cliente)
                .Include(h => h.Habitacion)
        );

        return reservas;
    }

    public async Task CambiarDeHabitacion(Guid reservaId, Guid habitacionNuevaId)
    {
        // Obtener la reserva por su ID
        var reserva = await ObtenerPorId(reservaId) ??
                      throw new CustomException
                          { Status = StatusCodes.Status404NotFound, Message = "No se exite reserva con ese id" };

        // Obtener la habitación por su ID
        var habitacion = await _repositorios.Habitacion.GetByIdAsync(habitacionNuevaId) ??
                         throw new CustomException
                         {
                             Message = "La habitación especificada no existe"
                         };


        if (habitacion.Estado == EstadoHabitacion.FueraDeServicio)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación especificada esta fuera de servicios en estos momentos"
            };

        // No permitir si no esta confirmada
        if (!reserva.LlegadaCliente)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "No ha sido confirmada la llegada del cliente para esta reserva."
            };
        }

        // Validar que la habitación no esté ocupada en las fechas indicadas (excluyendo la reserva actual)
        var habitacionOcupada = await _repositorios.Reserva.AnyAsync(h =>
                h.HabitacionId == habitacionNuevaId && // Misma habitación
                h.Id != reservaId && // Excluir la reserva actual
                !(h.FechaEntrada < DateTime.Now && !h.LlegadaCliente) && // Excluir reservas pasadas sin llegada
                !(reserva.FechaEntrada > h.FechaSalida || reserva.FechaSalida < h.FechaEntrada) // Se solapan
        );

        if (habitacionOcupada)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La habitación está ocupada en las fechas indicadas."
            };
        }

        // actulaizar reserva
        reserva.HabitacionId = habitacionNuevaId;

        _repositorios.Reserva.Update(reserva);

        // await SalvarCambios();
    }

    public async Task<List<Reserva>> ObtenerListaReservas() =>
        await _repositorios.Reserva
            .GetAllAsync(query => query
                .Include(c => c.Cliente)
                .Include(c => c.Habitacion));

    public async Task<List<Reserva>> ObtenerClientesYHabitacionesActivas(DateTime fecha)
    {
        if (fecha == default)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha especificada no es válida."
            };

        return await _repositorios.Reserva
            .GetAllAsync(r => r.FechaEntrada <= fecha && r.FechaSalida >= fecha,
                query => query.Include(c => c.Cliente).Include(c => c.Habitacion));
    }

    // todo: que si ya se le asigno la habitacion a otro no se pueda confirmar
    public async Task<Reserva> RegistrarLlegada(Guid reservaId)
    {
        var reserva = await ObtenerPorId(reservaId) ?? throw new CustomException
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "La reserva especificado no existe"
        };

        // Validar que no se pueda confirmar antes de el dia de entrada
        if (reserva.FechaEntrada < DateTime.Today && !reserva.LlegadaCliente)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de confirmación de esta reserva ya expiró"
            };
        }

        // Validar que no se pueda confirmar antes de el dia de entrada
        if (reserva.FechaEntrada > DateTime.Today && !reserva.LlegadaCliente)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El cliente no puede ser confirmado antes de la fecha de entrada"
            };
        }

        // Cambiar el estado de llegada del cliente
        reserva.LlegadaCliente = !reserva.LlegadaCliente;

        // Actualizar la reserva en la base de datos
        _repositorios.Reserva.Update(reserva);
        await SalvarCambios();

        return reserva;
    }

    public async Task<Reserva> CancelarReserva(Guid reservaId, string motivo)
    {
        var reserva = await ObtenerPorId(reservaId) ?? throw new CustomException
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "La reserva especificado no existe"
        };

        // No permitir si no esta confirmada
        if (!reserva.LlegadaCliente)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "No ha sido confirmada la llegada del cliente para esta reserva."
            };
        }

        // Si no está cancelada poner cancelación, motivo y fecha
        if (!reserva.EstaCancelada)
        {
            // Poner motivo
            reserva.MotivoCancelacion = motivo;
            // Guardar fecha
            reserva.FechaCancelacion = DateTime.Now;
            // Cambiar cancelacion de la reseva
            reserva.EstaCancelada = !reserva.EstaCancelada;
        }
        else //si esta cancelada revertir valores
        {
            // Validar que la habitación no esté ocupada en las fechas indicadas
            var habitacionOcupada = await _repositorios.Reserva.AnyAsync(h =>
                    h.HabitacionId == reserva.HabitacionId && // Misma habitación
                    !h.EstaCancelada && // Excluir reservas canceladas
                    !(h.FechaEntrada < DateTime.Now && !h.LlegadaCliente) && // Excluir reservas pasadas sin llegada
                    !(reserva.FechaEntrada > h.FechaSalida || reserva.FechaSalida < h.FechaEntrada) // Se solapan
            );

            if (habitacionOcupada)
            {
                throw new CustomException
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "La habitación está ocupada en las fechas indicadas."
                };
            }

            // Validar que el cliente no tenga otra reserva en las mismas fechas
            var clienteConReserva = await _repositorios.Reserva.AnyAsync(r =>
                r.ClienteId == reserva.ClienteId &&
                !r.EstaCancelada && // Excluir la que este cancelada
                !(reserva.FechaEntrada > r.FechaSalida || reserva.FechaSalida < r.FechaEntrada));

            if (clienteConReserva)
            {
                throw new CustomException
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "El cliente ya tiene una reserva en las fechas indicadas."
                };
            }

            // Validar que la habitación exista
            var habitacion = await _repositorios.Habitacion.GetByIdAsync(reserva.HabitacionId) ??
                             throw new CustomException
                             {
                                 Status = StatusCodes.Status400BadRequest,
                                 Message = "La habitación especificada no existe"
                             };

            if (habitacion.Estado is EstadoHabitacion.FueraDeServicio)
                throw new CustomException
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "La habitación especificada está fuera de servicio en estos momentos"
                };

            reserva.FechaCancelacion = default;
            reserva.EstaCancelada = false;
            reserva.MotivoCancelacion = null;
        }

        // Actualizar la reserva en la base de datos
        _repositorios.Reserva.Update(reserva);
        await SalvarCambios();

        return reserva;
    }

    private async Task<decimal> CalcularImporte(Reserva reserva)
    {
        //  Calcular importe (cant días * 10)
        var dias = reserva.FechaSalida - reserva.FechaEntrada; // Cambiar el orden de la resta
        var importe = (dias.Days + 1) * 10m;

        // Si es VIP, descontar el 10%
        var cliente = await _repositorios.Clientes.GetByIdAsync(reserva.ClienteId) ?? throw new CustomException
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "El cliente especificado no existe"
        };

        if (cliente.VIP) importe *= 0.9m; // Usar 0.9m para asegurar que es decimal

        reserva.Importe = importe;

        return importe;
    }
}