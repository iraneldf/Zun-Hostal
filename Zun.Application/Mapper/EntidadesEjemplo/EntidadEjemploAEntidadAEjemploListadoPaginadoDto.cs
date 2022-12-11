using AutoMapper;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper.EntidadesEjemplo
{
    public class EntidadEjemploAEntidadEjemploListadoPaginadoDto
 : Profile
    {
        public EntidadEjemploAEntidadEjemploListadoPaginadoDto
()
        {
            CreateMap<EntidadEjemplo, EntidadEjemploListadoPaginadoDto
>()
              .ReverseMap();
        }
    }
}
