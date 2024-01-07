using CommandsService.Models;

namespace CommandsService.Data
{
	public interface ICommandRepo
	{
		bool SaveChanges();

		#region PLATFORMS
		IEnumerable<Platform> GetAllPlatforms();
		void CreatePlatform(Platform platform);
		bool PlatformExists(int platformID);
		#endregion

		#region COMMANDS
		IEnumerable<Command> GetCommandsForPlatform(int platformID);
		Command GetCommand(int platformID, int commandID);
		void CreateCommand(int platformID, Command command);
		#endregion



	}
}
