using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Forums.ViewModel
{
    public class EditForum
    {
        public int Id { get; set; }
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
