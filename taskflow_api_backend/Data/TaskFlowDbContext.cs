using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models.Entities;

namespace TaskFlow.Api.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for TaskFlow API.
    /// Configures User and TaskItem entities and their relationships.
    /// </summary>
    public class TaskFlowDbContext : DbContext
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(50).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired();
            });

            // Task configuration
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).HasMaxLength(200).IsRequired();
                entity.Property(t => t.Description).HasMaxLength(4000);
                entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.UpdatedAt).IsRequired();

                entity.HasOne(t => t.AssignedToUser)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.AssignedToUserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
