using AutoMapper;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper.EntidadesEjemplo
{
    public class EntidadEjemploACrearEntidadEjemploDto : Profile
    {
        public EntidadEjemploACrearEntidadEjemploDto()
        {
            CreateMap<EntidadEjemplo, CrearEntidadEjemploInputDto>()
              .ReverseMap();
        }
    }
}
