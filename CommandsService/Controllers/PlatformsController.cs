using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
	[Route("api/c/[controller]/[action]")]
	[ApiController]
	public sealed class PlatformsController : ControllerBase
	{
		private readonly ICommandRepo repo;
		private readonly IMapper mapper;

		#region CTOR
		public PlatformsController(ICommandRepo repo, IMapper mapper)
		{
			this.repo = repo;
			this.mapper = mapper;
		}
		#endregion


		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDTO>> AllPlatforms()
		{
			Console.WriteLine(" ==> GETTING PLATFORMS FROM COMMANDS SERVICE");

			var platforms = repo.GetAllPlatforms();

			return Ok(mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
		}


		[HttpPost]
		public ActionResult TestConnection()
		{
			Console.WriteLine(" ==> POST INCOMING COMMAND SERVICE");
			return Ok("COMPLETED");
		}
	}
}
