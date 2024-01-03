using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public sealed class PlatformsController : ControllerBase
	{

		#region CTOR
		public PlatformsController()
		{
			
		}
		#endregion


		[HttpPost]
		public ActionResult TestConnection()
		{
			Console.WriteLine(" ==> POST INCOMING COMMAND SERVICE");
			return Ok("COMPLETED");
		}
	}
}
