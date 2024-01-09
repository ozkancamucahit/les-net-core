namespace CommandsService.EventProcessing
{
	public interface IEventProcesor
	{
		void ProcessEvent(string message);
	}
}
