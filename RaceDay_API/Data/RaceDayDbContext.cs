using Microsoft.EntityFrameworkCore;
using RaceDay_API.Models;
using RaceDay_API.Models;
using Route = RaceDay_API.Models.Route;

namespace RaceDay_API.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<WeatherCache> WeatherCaches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => new { c.EventId, c.Name }).IsUnique();
            modelBuilder.Entity<Enrolment>().HasIndex(e => new { e.UserId, e.EventId }).IsUnique();

            // One-to-One: Enrolment -> Result
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Enrolment)
                .WithOne(e => e.Result)
                .HasForeignKey<Result>(r => r.EnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent cascade delete conflicts on Event deletion
            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.Event)
                .WithMany(ev => ev.Enrolments)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fix the cascade delete cycle error
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
