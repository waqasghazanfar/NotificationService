namespace NotificationService.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Domain.Entities;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure NotificationLog
            modelBuilder.Entity<NotificationLog>(b =>
            {
                b.HasKey(nl => nl.Id);
                b.HasIndex(nl => nl.CorrelationId);
                b.Property(nl => nl.EventName).IsRequired().HasMaxLength(100);
                b.Property(nl => nl.Channel).IsRequired().HasMaxLength(50);
                b.Property(nl => nl.Recipient).IsRequired().HasMaxLength(256);
                b.Property(nl => nl.Status).IsRequired().HasMaxLength(50);
            });

            // Configure Template
            modelBuilder.Entity<Template>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => new { t.Name, t.Channel, t.Locale }).IsUnique();
                b.Property(t => t.Name).IsRequired().HasMaxLength(100);
                b.Property(t => t.Channel).IsRequired().HasMaxLength(50);
                b.Property(t => t.Locale).IsRequired().HasMaxLength(10);
                b.Property(t => t.Subject).HasMaxLength(255);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}