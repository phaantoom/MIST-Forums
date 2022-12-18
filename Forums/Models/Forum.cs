using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forums.Models
{
    public class Forum
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        //[ForeignKey("level")]
        public int levelId { get; set; }
        public virtual Level level { get; set; }
        public virtual ICollection<UserForum> UserForum { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Forum()
        {
            UserForum = new List<UserForum>();
        }
    }
}
