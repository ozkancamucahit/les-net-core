
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOS;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repo;
        private readonly IMapper mapper;
		private readonly ICommandDataClient commandDataClient;
		private readonly IMessageBusClient messageBusClient;

		#region CTOR
		public PlatformsController(
                    IPlatformRepo repo,
                    IMapper mapper,
                    ICommandDataClient commandDataClient,
                    IMessageBusClient messageBusClient)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
			this.messageBusClient = messageBusClient;
		}
        #endregion

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

            // SEND SYNC MESSAGE


            try
            {
                await commandDataClient.SendPlatformToCommand(PlatformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ==> COULD NOT SEND SYNCHRONOUSLY" + ex.Message);
            }

            // SEND AYSNC MESSAGE
            try
            {
                var platformPublishedDTO = mapper.Map<PlatformPublishedDTO>(PlatformReadDto);
                platformPublishedDTO.Event = "Platform_Published";
                messageBusClient.PublishNewPlatform(platformPublishedDTO);
            }
            catch (Exception ex)
			{
				Console.WriteLine(" ==> COULD NOT SEND ASYNCHRONOUSLY" + ex.Message);
			}

			return CreatedAtRoute(nameof(GetPlatformById), new { Id = PlatformReadDto.Id}, PlatformReadDto);

        }

    }
}


