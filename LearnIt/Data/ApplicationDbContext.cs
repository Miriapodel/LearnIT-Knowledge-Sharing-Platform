using LearnIt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnIt.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<MyUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Like>()
                .HasKey(ab => new { ab.Id, ab.CommentId, ab.MyUserId });

            modelBuilder.Entity<Like>()
                .HasOne(ab => ab.Comment)
                .WithMany(ab => ab.Likes)
                .HasForeignKey(ab => ab.CommentId);

            modelBuilder.Entity<Like>()
                .HasOne(ab => ab.MyUser)
                .WithMany(ab => ab.Likes)
                .HasForeignKey(ab => ab.MyUserId);
        }
    }
}