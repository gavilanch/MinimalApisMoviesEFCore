using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetGenres)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository repository,
            IMapper mapper)
        {
            var genres = await repository.GetAll();
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            return TypedResults.Ok(genresDTO);
        }

        static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, 
            IGenresRepository repository, IMapper mapper)
        {
            var genre = await repository.GetById(id);

            if (genre is null)
            {
                return TypedResults.NotFound();
            }

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Ok(genreDTO);
        }

        static async Task<Created<GenreDTO>> Create(CreateGenreDTO createGenreDTO, 
            IGenresRepository repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var genre = mapper.Map<Genre>(createGenreDTO);

            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Created($"/genres/{id}", genreDTO);
        }

        static async Task<Results<NotFound, NoContent>> Update(int id, 
            CreateGenreDTO createGenreDTO,
            IGenresRepository repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var genre = mapper.Map<Genre>(createGenreDTO);
            genre.Id = id;

            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }
    }
}
