using AutoMapper;
using Zun.Application.Mapper.EntidadesEjemplo;

namespace Zun.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

        }

        public static List<Profile> GetProfiles()
        {
            return new List<Profile>
            {
                #region EntidadEjemplo
                    new EntidadEjemploAEntidadEjemploDto(),
                    new EntidadEjemploAEditarEntidadEjemploDto(),
                    new EntidadEjemploACrearEntidadEjemploDto(),
                #endregion
            };
        }
    }
}