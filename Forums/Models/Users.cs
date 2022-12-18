using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forums.Models
{
    public class Users:IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public bool IsMempershipPaid { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        
        //[ForeignKey("level")]
        public int levelId { get; set; }
        public virtual Level level { get; set; }
        public virtual ICollection<UserForum> userForum { get; set; }
    }
}
