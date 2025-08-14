using Microsoft.EntityFrameworkCore;
using taskflow_api_backend.Models;

namespace taskflow_api_backend.Data
{
    /// <summary>
    /// Application EF Core DbContext for TaskFlow.
    /// </summary>
    public class AppDbContext : DbContext
    {
        // PUBLIC_INTERFACE
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Users table.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Tasks table.
        /// </summary>
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(50).HasDefaultValue("User");
            });

            // TaskItem
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(4000);
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.UpdatedAt).IsRequired();

                entity.HasOne(t => t.CreatedBy)
                      .WithMany(u => u.CreatedTasks)
                      .HasForeignKey(t => t.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedTo)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(t => t.AssignedToId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
