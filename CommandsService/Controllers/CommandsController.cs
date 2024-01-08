using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOS;
using CommandsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace CommandsService.Controllers
{
	[Route("api/c/platforms/{platformId}/[controller]/[action]")]
	[ApiController]
	public sealed class CommandsController : ControllerBase
	{
		private readonly ICommandRepo repo;
		private readonly IMapper mapper;

		#region CTOR
		public CommandsController(ICommandRepo repo, IMapper mapper)
        {
			this.repo = repo;
			this.mapper = mapper;
		}
		#endregion


		[HttpGet]
		public ActionResult<IEnumerable<CommandReadDTO>> CommandsForPlatform(int platformId) 
		{

			Console.WriteLine(" ==> GET COMMANDS FOR PLATFORM ID:" + platformId);

			if(!repo.PlatformExists(platformId))
			{
				return NotFound();
			}

			var commands = repo.GetCommandsForPlatform(platformId);

			return Ok(mapper.Map<IEnumerable<CommandReadDTO>>(commands));
		}

		[HttpGet("{commandId}", Name = "GetCommandForPlatform")]
		public ActionResult<CommandReadDTO> GetCommandForPlatform(int platformId, int commandId) 
		{
			Console.WriteLine(" ==> GET COMMAND FOR PLATFORM ID :" + platformId + " COMMAND ID :" + commandId );

			if (!repo.PlatformExists(platformId))
			{
				return NotFound();
			}

			var command = repo.GetCommand(platformId, commandId);

			if (command == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<CommandReadDTO>(command));

		}

		[HttpPost]
		public ActionResult<CommandReadDTO> CreateCommandforPlatform(int platformId, CommandCreateDTO commandDTO)
		{
			Console.WriteLine(" ==> POST CREATE COMMAND FOR PLATFORM ID :" + platformId + " COMMAND LINE :" + commandDTO.CommandLine);

			if (!repo.PlatformExists(platformId))
			{
				return NotFound();
			}

			if(ModelState.IsValid)
			{
				var command = mapper.Map<Command>(commandDTO);
				repo.CreateCommand(platformId, command);

				if (repo.SaveChanges())
				{
					var commandReadDTO = mapper.Map<CommandReadDTO>(command);

					return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId, commandId = commandReadDTO.Id}, commandReadDTO);
				}
				else
				{
					return BadRequest("COULD NOT SAVE CHANGES CHECK MODEL");
				}
			}
			else
			{
				return BadRequest(ModelState);
			}
		}




	}
}
