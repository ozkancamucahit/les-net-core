using AutoMapper;
using CommandsService.DTOS;
using CommandsService.Models;

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
		}
    }
}
