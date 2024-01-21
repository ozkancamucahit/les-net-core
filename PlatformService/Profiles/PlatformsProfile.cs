using AutoMapper;
using PlatformService.DTOS;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public sealed class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // Source => Target 

            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDTO>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(target => target.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(target => target.Publisher, opt => opt.MapFrom(src => src.Publisher));

        }

        


    }
}