using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Models
{
    public class EventManagementDbContext : DbContext
    {
        public EventManagementDbContext(DbContextOptions<EventManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure EventAttendee as join entity for many-to-many
            modelBuilder.Entity<EventAttendee>()
                .HasKey(ea => new { ea.EventId, ea.AttendeeId });

            modelBuilder.Entity<EventAttendee>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.EventAttendees)
                .HasForeignKey(ea => ea.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventAttendee>()
                .HasOne(ea => ea.Attendee)
                .WithMany(a => a.EventAttendees)
                .HasForeignKey(ea => ea.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Event>()
                .Property(e => e.Location)
                .IsRequired().HasMaxLength(200);

            modelBuilder.Entity<Event>()
                .Property(e => e.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(e => e.Capacity)
                .IsRequired();

            modelBuilder.Entity<Attendee>()
                .Property(a => a.Name)
                .IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Attendee>()
                .Property(a => a.Email)
                .IsRequired().HasMaxLength(150);
        }
    }
}