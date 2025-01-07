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

        return await _repositorios.BasicRepository.AddAsync(EstablecerDatosAuditoria(reserva));
    }

    public override async Task<EntityEntry<Reserva>> Actualizar(Reserva reserva)
    {
        await ValidarAntesActualizar(reserva);

        reserva.Importe = await CalcularImporte(reserva);

        return _repositorios.BasicRepository.Update(EstablecerDatosAuditoria(reserva, false));
    }

    // todo: probar
    public override async Task ValidarAntesCrear(Reserva reserva)
    {
        await base.ValidarAntesCrear(reserva);

        var condicion = await _repositorios.Reserva.AnyAsync(h =>
            h.HabitacionId == reserva.HabitacionId &&
            (h.FechaEntrada > reserva.FechaSalida || h.FechaSalida < reserva.FechaEntrada));

        if (!condicion)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = $"La habitación {reserva.Habitacion.Numero} está ocupada en las fechas indicadas."
            };
        }

        if (reserva.FechaSalida < DateTime.Now || reserva.FechaEntrada < DateTime.Now)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message =
                    $"La fecha de entrada y salida deben ser posteriores a la actual {DateTime.Now} > {reserva.FechaEntrada}"
            };

        var diferencia = reserva.FechaSalida - reserva.FechaEntrada;


        if (reserva.FechaEntrada >= reserva.FechaSalida)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de entrada no puede ser mayor o igual que la fecha de salida"
            };

        if (Math.Abs(diferencia.Days) < 2)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El periodo mínimo para realizar la reserva es de 3 días"
            };


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
                Message = "La habitación especificada esta fuera de servicio en estos momentos"
            };
    }

    public override async Task ValidarAntesActualizar(Reserva reserva)
    {
        /*if (reserva.FechaSalida > DateTime.Now || reserva.FechaEntrada > DateTime.Now)
        {
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de entrada y salida deben ser posteriores a la actual"
            };
        }*/

        if (!await _repositorios.Reserva.AnyAsync(e => e.Id == reserva.Id))
            throw new CustomException
                { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

        var diferencia = reserva.FechaSalida - reserva.FechaEntrada;

        if (reserva.FechaEntrada >= reserva.FechaSalida)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "La fecha de entrada no puede ser menor o igual que la fecha de salida"
            };

        if (Math.Abs(diferencia.Days) < 2)
            throw new CustomException
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "El periodo mínimo para realizar la reserva es de 3 días"
            };

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
                Message = "La habitación especificada esta fuera de servicio en estos momentos"
            };

        //validando los datos insertados por el Rol
        await new ReservaValidator(_repositorios).ValidateAndThrowAsync(reserva);

        //validando reglas de negocios
    }

    public override async Task<EntityEntry<Reserva>> Eliminar(Guid reservaId)
    {
        // sobreescribir para poner la habitaicon asignada en Desocupada
        var reserva = await ObtenerPorId(reservaId) ??
                      throw new CustomException
                          { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

        // En caso de que la resverva este vigente desocupar la habitacion
        if (reserva.FechaEntrada >= DateTime.Now && reserva.FechaSalida <= DateTime.Now)
            await DesOcuparHabitacion(reserva.HabitacionId);

        return await base.Eliminar(reservaId);
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

        var habitacionAntigaId = reserva.HabitacionId;

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

        // actulaizar reserva
        reserva.HabitacionId = habitacionNuevaId;

        // en este metodo se ocupa la habitacion nueva
        await Actualizar(reserva);

        // desocupar antigua habitacion
        await DesOcuparHabitacion(habitacionAntigaId);

        await SalvarCambios();
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

    public async Task<Reserva> RegistrarLlegada(Guid reservaId)
    {
        var reserva = await ObtenerPorId(reservaId) ?? throw new CustomException
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "La reserva especificado no existe"
        };

        reserva.LlegadaCliente = true;

        _repositorios.Reserva.Update(reserva);

        await SalvarCambios();

        return reserva;
    }

    private async Task<decimal> CalcularImporte(Reserva reserva)
    {
        //  Calcular importe (cant días * 10)
        var dias = reserva.FechaSalida - reserva.FechaEntrada; // Cambiar el orden de la resta
        var importe = dias.Days * 10m;

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

    private async Task DesOcuparHabitacion(Guid habitacionId)
    {
        // Obtener la habitación por su ID
        var habitacion = await _repositorios.Habitacion.GetByIdAsync(habitacionId);

        if (habitacion == null)
            throw new CustomException
                { Status = StatusCodes.Status404NotFound, Message = "No se existe esa habitación" };

        // Modificar la propiedad
        habitacion.Estado = EstadoHabitacion.Disponible;

        _repositorios.Habitacion.Update(habitacion);

        await SalvarCambios();
    }
}
