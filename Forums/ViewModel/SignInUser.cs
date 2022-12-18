using System.ComponentModel.DataAnnotations;

namespace Forums.ViewModel
{
    public class SignInUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
