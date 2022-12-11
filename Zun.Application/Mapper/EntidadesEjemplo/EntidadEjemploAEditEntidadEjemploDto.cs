using AutoMapper;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper.EntidadesEjemplo
{
    public class EntidadEjemploAEditarEntidadEjemploDto : Profile
    {
        public EntidadEjemploAEditarEntidadEjemploDto()
        {
            CreateMap<EntidadEjemplo, ModificarEntidadEjemploInputDto>()
              .ReverseMap();
        }
    }
}
