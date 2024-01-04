
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOS;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repo;
        private readonly IMapper mapper;
		private readonly ICommandDataClient commandDataClient;

		public PlatformsController(
			IPlatformRepo repo,
			IMapper mapper,
			ICommandDataClient commandDataClient)
        {
            this.repo = repo;
            this.mapper = mapper;
			this.commandDataClient = commandDataClient;
		}

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms(){
            
            Console.WriteLine("==> Gettting Platforms");

            IEnumerable<Platform> platformItems = repo.GetAllPlatforms();

            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));

        }

        [HttpGet("{id:int:range(1,2147483647)}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id){

            var platformItem = repo.GetPlatformById(id);

            if (platformItem != null)
                return Ok(mapper.Map<PlatformReadDto>(platformItem));

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto){

            Platform platformModel = mapper.Map<Platform>(platformCreateDto);

            repo.CreatePlatform(platformModel);
            repo.SaveChanges();

            var PlatformReadDto = mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await commandDataClient.SendPlatformToCommand(PlatformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ==> COULD NOT CREATE" + ex.Message);
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = PlatformReadDto.Id}, PlatformReadDto);

        }

    }
}


