using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
           ErrorMessage = "Password and confirmation password do not match.")]
        public string Confirm_Password { get; set; }
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
