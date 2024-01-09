using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOS;
using CommandsService.Models;
using System.Text.Json;

namespace CommandsService.EventProcessing
{

	public enum EventType
	{
		PlatformPublished,
		Undetermined
	}

	public sealed class EventProcesor : IEventProcesor
	{
		private readonly IServiceScopeFactory scopeFactory;
		private readonly IMapper mapper;

		#region CTOR
		public EventProcesor(IServiceScopeFactory scopeFactory,
			IMapper mapper)
        {
			this.scopeFactory = scopeFactory;
			this.mapper = mapper;
		}
        #endregion

        public void ProcessEvent(string message)
		{
			EventType eventType = DetermineEvent(message);

			switch (eventType)
			{
				case EventType.PlatformPublished:
					AddPlatform(message);
					break;
				case EventType.Undetermined:
					break;
				default:
					break;
			}

		}

		private void AddPlatform(string  platformPublishedMessage)
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

				var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

				try
				{
					var plat = mapper.Map<Platform>(platformPublishedDTO);

					if(!repo.ExternalPlatformExists(plat.ExternalID))
					{
						repo.CreatePlatform(plat);
						repo.SaveChanges();
                        Console.WriteLine(" ==> ADDED PLATFORM");
                    }
					else
					{
                        Console.WriteLine(" ==> PLATFORM ALREADY EXISTS");
                    }

				}
				catch (Exception ex)
				{
                    Console.WriteLine(" ==> COULD NOT ADD PLATFORM TO DB :" + ex.Message);
                    throw;
				}

			}
		}

		private EventType DetermineEvent(string NotificationMessage)
		{
            Console.WriteLine(" ==> DETERMINING EVENT");

			var eventType = JsonSerializer.Deserialize<GenericEventDTO>(NotificationMessage);

			switch (eventType.Event)
			{
				case "Platform_Published":
                    Console.WriteLine(" ==> PLATFORM PUBLISHED EVENT DETECTED");
					return EventType.PlatformPublished;

				default:
                    Console.WriteLine(" ==> COULD NOT DETERMINE EVENT TYPE");
					return EventType.Undetermined;
			}

		}

	}
}
