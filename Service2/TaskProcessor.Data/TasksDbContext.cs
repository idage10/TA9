using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Data
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options) { }
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Children)
                .WithOne(t => t.Parent)
                .HasForeignKey(t => t.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}