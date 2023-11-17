
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOS;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repo;
        private readonly IMapper mapper;

        public PlatformsController(IPlatformRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
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
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto){

            Platform platformModel = mapper.Map<Platform>(platformCreateDto);

            repo.CreatePlatform(platformModel);
            repo.SaveChanges();

            var PlatformReadDto = mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = PlatformReadDto.Id}, PlatformReadDto);

        }

    }
}


