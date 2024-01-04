using PlatformService.DTOS;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
	public sealed class HttpCommandDataClient : ICommandDataClient
	{
		#region FIELDS
		private readonly HttpClient _httpClient;
		private readonly IConfiguration configuration;

		#endregion

		#region CTOR
		public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
			this.configuration = configuration;
		}
        #endregion
        public async Task SendPlatformToCommand(PlatformReadDto platform)
		{
			var httpContent = new StringContent(
					JsonSerializer.Serialize(platform),
					encoding: Encoding.UTF8,
					mediaType: "application/json"
				);

			var response = await _httpClient.PostAsync(configuration["CommandService"], httpContent);

			if(response.IsSuccessStatusCode)
			{
				Console.WriteLine(" ==> POST : OK");
			}
			else
			{

				Console.WriteLine(" ==> POST : NOT OK");
			}

		}
	}
}
