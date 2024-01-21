using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
	public sealed class PlatformDataClient : IPlatformDataClient
	{
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;
		private readonly string grpcPlatformEndPoint;


		#region CTOR
		public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
			this._configuration = configuration;
			this._mapper = mapper;
			grpcPlatformEndPoint = _configuration["GrpcPlatform"];
		}
        #endregion


        public IEnumerable<Platform> ReturnAllPlatforms()
		{
            Console.WriteLine(" ==> CALLING GRPC SERVICE :" + grpcPlatformEndPoint);

			var channel = GrpcChannel.ForAddress(grpcPlatformEndPoint);
			var client = new GrpcPlatform.GrpcPlatformClient(channel);
			var request = new GetAllRequest();

			try
			{
				var reply = client.GetAllPlatforms(request);
				return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
			}
			catch (Exception ex)
			{
                Console.WriteLine(" ==> EXCEPTION : COULD NOT CALL GRPC SERVER :" + ex.Message);
                return Enumerable.Empty<Platform>();
			}


        }
	}
}
