namespace NotificationService.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Domain.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<SmtpSetting> SmtpSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure NotificationLog
            modelBuilder.Entity<NotificationLog>(b =>
            {
                b.HasKey(nl => nl.Id);
                b.HasIndex(nl => nl.CorrelationId);
                b.HasIndex(nl => nl.UserId); // <-- ADDED INDEX
                b.Property(nl => nl.EventName).IsRequired().HasMaxLength(100);
                b.Property(nl => nl.Channel).IsRequired().HasMaxLength(50);
                b.Property(nl => nl.Recipient).IsRequired().HasMaxLength(256);
                b.Property(nl => nl.Status).IsRequired().HasMaxLength(50);
                b.Property(nl => nl.Priority).IsRequired().HasMaxLength(10).HasDefaultValue("Low");
                b.HasIndex(nl => new { nl.Status, nl.Priority, nl.ScheduleAtUtc });
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

            // Configure SmtpSetting
            modelBuilder.Entity<SmtpSetting>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Host).IsRequired().HasMaxLength(255);
                b.Property(s => s.Username).IsRequired().HasMaxLength(255);
                b.Property(s => s.Password).IsRequired();
                b.Property(s => s.FromAddress).IsRequired().HasMaxLength(255);
                b.Property(s => s.FromName).IsRequired().HasMaxLength(255);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
