using System.ComponentModel.DataAnnotations;

namespace Forums.Models
{
    public class Level
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string TypeName { get; set; }
        public virtual ICollection<Users> user { get; set; }
        public virtual ICollection<Forum> forum { get; set; }
    }
}
