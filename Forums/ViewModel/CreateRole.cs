using System.ComponentModel.DataAnnotations;

namespace Forums.ViewModel
{
    public class CreateRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
