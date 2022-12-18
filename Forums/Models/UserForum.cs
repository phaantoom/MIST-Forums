using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forums.Models
{
    public class UserForum
    {
        public int Id { get; set; }
        public int forumId { get; set; }
        public virtual Forum forum { get; set; }
        public string userId { get; set; }
        public virtual Users user { get; set; }
        public int? ParentId { get; set; }
        public virtual UserForum Parent { get; set; }
        public virtual ICollection<UserForum> Replies { get; set; } = new List<UserForum>();
        [StringLength(500)]
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
