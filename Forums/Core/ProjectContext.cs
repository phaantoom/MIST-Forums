using Forums.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forums.Core
{
    public class ProjectContext : IdentityDbContext<Users>
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }
        public override DbSet<Users> Users { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<UserForum> UserForums { get; set; }
        public DbSet<Level> levels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserForum>()
                .HasOne(x => x.user)
                .WithMany(u => u.userForum)
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserForum>()
                .HasOne(x => x.forum)
                .WithMany(u => u.UserForum)
                .HasForeignKey(x => x.forumId);

            modelBuilder.Entity<UserForum>()
                .HasOne(x => x.Parent)
                .WithMany(u => u.Replies)
                .IsRequired(false)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Users>()
                .HasOne(x => x.level)
                .WithMany(u => u.user)
                .HasForeignKey(x => x.levelId);

            modelBuilder.Entity<Forum>()
                .HasOne(x => x.level)
                .WithMany(u => u.forum)
                .HasForeignKey(x => x.levelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
