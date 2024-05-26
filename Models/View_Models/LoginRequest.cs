using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
