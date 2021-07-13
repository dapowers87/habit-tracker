using AutoMapper;

namespace Application.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Persistence.Entities.Window, Domain.ApiModels.WindowWithoutUser>();
        }
    }
}