using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
