using Forums.Models;
using System.ComponentModel.DataAnnotations;

namespace Forums.ViewModel
{
    public class CreateUser
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public int levelId { get; set; }
        public string RoleId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmPassword don't match")]
        public string ConfirmPassword { get; set; }
    }
}
