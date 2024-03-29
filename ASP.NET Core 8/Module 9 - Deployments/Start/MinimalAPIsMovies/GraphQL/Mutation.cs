using AutoMapper;
using HotChocolate.Authorization;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.GraphQL
{
    public class Mutation
    {
        [Serial]
        [Authorize(Policy = "isadmin")]
        public async Task<GenreDTO> InsertGenre([Service] IGenresRepository repository,
            [Service] IMapper mapper, CreateGenreDTO createGenreDTO)
        {
            var genre = mapper.Map<Genre>(createGenreDTO);
            await repository.Create(genre);
            var genreDTO = mapper.Map<GenreDTO>(genre);
            return genreDTO;
        }
    }
}
