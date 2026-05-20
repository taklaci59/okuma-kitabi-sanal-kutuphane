using Microsoft.EntityFrameworkCore;
using okumatakibisanalkutuphane.Models;

namespace okumatakibisanalkutuphane.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ReadingSession> ReadingSessions { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<PersonnelProfile> PersonnelProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
