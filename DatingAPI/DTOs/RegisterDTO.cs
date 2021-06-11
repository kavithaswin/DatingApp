using System.ComponentModel.DataAnnotations;

namespace DatingAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username  { get; set; }
        [Required]
        [StringLength(8,MinimumLength =5)]
        public string Password { get; set; }
    }
}