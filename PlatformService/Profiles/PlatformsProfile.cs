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

        }

        


    }
}