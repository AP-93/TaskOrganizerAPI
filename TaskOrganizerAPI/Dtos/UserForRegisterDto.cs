using System.ComponentModel.DataAnnotations;

namespace TaskOrganizerAPI.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20,MinimumLength =6)]
        public string Password { get; set; }
    }
}
