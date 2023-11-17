
using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOS
{
    public sealed class PlatformCreateDto
    {
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string Publisher { get; set; } = String.Empty;
        [Required]
        public string Cost { get; set; } = String.Empty;
    }
}


