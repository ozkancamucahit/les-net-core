using AutoMapper;
using CommandsService.DTOS;
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
					break;
				case EventType.Undetermined:
					break;
				default:
					break;
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
