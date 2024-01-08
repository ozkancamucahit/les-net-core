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
		}
    }
}
