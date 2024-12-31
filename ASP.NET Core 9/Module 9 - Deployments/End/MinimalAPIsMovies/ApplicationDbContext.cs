using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entities;
using Error = MinimalAPIsMovies.Entities.Error;

namespace MinimalAPIsMovies
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new Genre
            {
                Id = 1,
                Name = "Action"
            });

            modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);

            modelBuilder.Entity<Actor>().Property(p => p.Name).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(p => p.Picture).IsUnicode();

            modelBuilder.Entity<Movie>().Property(p => p.Title).HasMaxLength(250);
            modelBuilder.Entity<Movie>().Property(p => p.Poster).IsUnicode();

            modelBuilder.Entity<GenreMovie>().HasKey(gm => new { gm.MovieId, gm.GenreId });

            modelBuilder.Entity<ActorMovie>().HasKey(am => new { am.MovieId, am.ActorId });

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims"); 
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");


        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GenreMovie> GenresMovies { get; set; }
        public DbSet<ActorMovie> ActorsMovies { get; set; }
        public DbSet<Error> Errors { get; set; }
    }
}
