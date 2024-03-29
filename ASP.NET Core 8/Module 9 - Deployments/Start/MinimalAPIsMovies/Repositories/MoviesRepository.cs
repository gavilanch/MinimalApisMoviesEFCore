using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using System.Linq.Dynamic.Core;

namespace MinimalAPIsMovies.Repositories
{
    public class MoviesRepository(IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context, IMapper mapper, ILogger<MoviesRepository> logger) : IMoviesRepository
    {
        public async Task<List<Movie>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Movies.AsQueryable();
            await httpContextAccessor.HttpContext!
                .InsertPaginationParameterInResponseHeader(queryable);
            return await queryable.OrderBy(m => m.Title).Paginate(pagination)
                .ToListAsync();
        }

        public async Task<Movie?> GetById(int id)
        {
            return await context.Movies
                .Include(m => m.Comments)
                .Include(m => m.GenresMovies)
                    .ThenInclude(gm => gm.Genre)
                .Include(m => m.ActorsMovies.OrderBy(am => am.Order))
                    .ThenInclude(am => am.Actor)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> Create(Movie movie)
        {
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        public async Task Update(Movie movie)
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Movies.Where(m => m.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task Assign(int id, List<int> genresIds)
        {
            var movie = await context.Movies.Include(m => m.GenresMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
            {
                throw new ArgumentException($"There's no movie with id {id}");
            }

            var genresMovies = genresIds.Select(genreId => new GenreMovie { GenreId = genreId });

            movie.GenresMovies = mapper.Map(genresMovies, movie.GenresMovies);

            await context.SaveChangesAsync();
        }

        public async Task Assign(int id, List<ActorMovie> actors)
        {
            for (int i = 1; i <= actors.Count; i++)
            {
                actors[i - 1].Order = i;
            }

            var movie = await context.Movies.Include(m => m.ActorsMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
            {
                throw new ArgumentException($"There's not movie with id {id}");
            }

            movie.ActorsMovies = mapper.Map(actors, movie.ActorsMovies);

            await context.SaveChangesAsync();
        }

        public async Task<List<Movie>> Filter(MoviesFilterDTO moviesFilterDTO)
        {
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(moviesFilterDTO.Title))
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.Title.Contains(moviesFilterDTO.Title));
            }

            if (moviesFilterDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (moviesFilterDTO.FutureReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (moviesFilterDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.GenresMovies
                        .Select(y => y.GenreId)
                        .Contains(moviesFilterDTO.GenreId));
            }

            if (!string.IsNullOrEmpty(moviesFilterDTO.OrderByField))
            {
                var orderKind = moviesFilterDTO.OrderByAscending ? "ascending" : "descending";

                try
                {
                    // title ascending, title descending, releaseDate ascending
                    moviesQueryable = moviesQueryable
                        .OrderBy($"{moviesFilterDTO.OrderByField} {orderKind}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }


            await httpContextAccessor.HttpContext!
                .InsertPaginationParameterInResponseHeader(moviesQueryable);

            var movies = await moviesQueryable.Paginate(moviesFilterDTO.PaginationDTO)
                .ToListAsync();

            return movies;
        }
    }
}
