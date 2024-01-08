using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOS
{
	public sealed class CommandCreateDTO
	{
		[Required]
		public string HowTo { get; set; } = string.Empty;

		[Required]
		public string CommandLine { get; set; } = string.Empty;
	}
}
