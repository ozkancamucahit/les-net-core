using CommandsService.Models;

namespace CommandsService.Data
{
	public sealed class CommandRepo : ICommandRepo
	{
		private readonly AppDbContext context;

		#region CTOR
		public CommandRepo( AppDbContext context)
        {
			this.context = context;
		}
        #endregion

        public void CreateCommand(int platformID, Command command)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}

			command.PlatformId = platformID;
			context.Commands.Add(command);
		}

		public void CreatePlatform(Platform platform)
		{
			if ( platform == null)
			{
				throw new ArgumentNullException(nameof(platform));
			}

			context.Platforms.Add(platform);
		}

		public IEnumerable<Platform> GetAllPlatforms()
		{
			return context.Platforms;
		}

		public Command GetCommand(int platformID, int commandID)
		{
			return context.Commands
				.Where(c => c.PlatformId == platformID && c.Id == commandID).FirstOrDefault();
		}

		public IEnumerable<Command> GetCommandsForPlatform(int platformID)
		{
			return context.Commands
				.Where(c => c.PlatformId == platformID)
				.OrderBy(c => c.Platform.Name);
		}

		public bool PlatformExists(int platformID)
		{
			return context.Platforms.Any(p => p.Id == platformID);
		}

		public bool ExternalPlatformExists(int externalPlatformID)
		{
			return context.Platforms.Any(p => p.ExternalID == externalPlatformID);
		}

		public bool SaveChanges()
		{
			return context.SaveChanges() >= 0;
		}
	}
}
