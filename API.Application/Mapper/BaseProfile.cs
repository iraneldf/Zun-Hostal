using API.Application.Dtos.Comunes;
using API.Data.Entidades;
using AutoMapper;

namespace API.Application.Mapper
{

    public class BaseProfile<Entity, EntityDto, CrearEntityInputDto, ActualizarEntityInputDto, ListEntityDto> : Profile
    where Entity : EntidadBase where EntityDto : EntidadBaseDto where ListEntityDto : EntidadBaseDto
    {
        public BaseProfile()
        {
            MapEntityDto();
            MapCrearEntityDto();
            MapActualizarEntityDto();
            MapListEntityDto();
        }



        public virtual void MapListEntityDto()
        {
            CreateMap<Entity, ListEntityDto>().ReverseMap();
        }

        public virtual void MapActualizarEntityDto()
        {
            CreateMap<Entity, ActualizarEntityInputDto>().ReverseMap();
        }

        public virtual void MapCrearEntityDto()
        {
            CreateMap<Entity, CrearEntityInputDto>().ReverseMap();
        }

        public virtual void MapEntityDto()
        {
            CreateMap<Entity, EntityDto>().ReverseMap();
        }
    }
}
