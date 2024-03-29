using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.GraphQL
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Genre> GetGenres([Service] ApplicationDbContext context) 
            => context.Genres;

        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Actor> GetActors([Service] ApplicationDbContext context)
            => context.Actors;

        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Movie> GetMovies([Service] ApplicationDbContext context)
            => context.Movies;
    }
}
