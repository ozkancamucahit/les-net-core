using AutoMapper;
using CommandsService.DTOS;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{
	public sealed class CommandsProfile : Profile
	{
        public CommandsProfile()
        {
			// Source => Target 

			CreateMap<Platform, PlatformReadDTO>();
			CreateMap<Command, CommandReadDTO>();
			CreateMap<CommandCreateDTO, Command>();
			CreateMap<PlatformPublishedDTO, Platform>()
				.ForMember(target => target.ExternalID, opt => opt.MapFrom(src => src.Id));
			CreateMap<GrpcPlatformModel, Platform>()
				.ForMember(target => target.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
				.ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(target => target.Commands, opt => opt.Ignore());
		}
    }
}
