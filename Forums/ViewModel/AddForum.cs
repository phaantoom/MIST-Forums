using Forums.Models;
using System.ComponentModel.DataAnnotations;

namespace Forums.ViewModel
{
    public class AddForum
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Forum level")]
        public int levelId { get; set; }
        [StringLength(250)]
        [Required]
        public string Description { get; set; }
    }
}
