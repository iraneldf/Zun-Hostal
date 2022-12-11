using AutoMapper;
using Zun.Application.Dtos.EntidadEjemplo;
using Zun.Data.Entidades;

namespace Zun.Application.Mapper.EntidadesEjemplo
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
