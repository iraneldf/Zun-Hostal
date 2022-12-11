using AutoMapper;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper.EntidadesEjemplo
{
    public class EntidadEjemploAEntidadEjemploDto : Profile
    {
        public EntidadEjemploAEntidadEjemploDto()
        {
            CreateMap<EntidadEjemplo, EntidadEjemploDto>()
              .ReverseMap();
        }
    }
}
