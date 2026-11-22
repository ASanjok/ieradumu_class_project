using System;
using System.Collections.Generic;
using System.Text;
using class_project_ieradumi;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Habit> Habits => Set<Habit>();
        public DbSet<CompletionRecord> CompletionRecords => Set<CompletionRecord>();
        public DbSet<HabitSchedule> HabitSchedules => Set<HabitSchedule>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Habit>(entity =>
            {
                entity.HasKey(h => h.ID);
                entity.Property(h => h.HabitName).IsRequired();
            });

            modelBuilder.Entity<CompletionRecord>(entity =>
            {
                entity.HasKey(c => c.ID);
            });

            modelBuilder.Entity<HabitSchedule>(entity =>
            {
                entity.HasKey(c => c.ID);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(c => c.ID);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(c => c.ID);
            });
        }
    }
}
