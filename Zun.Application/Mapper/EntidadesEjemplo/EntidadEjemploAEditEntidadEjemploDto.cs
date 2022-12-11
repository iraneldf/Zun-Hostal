using AutoMapper;
using Zun.Application.Dtos.EntidadEjemplo;
using Zun.Data.Entidades;

namespace Zun.Application.Mapper.EntidadesEjemplo
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
