using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.DTOs
{
    public class CreateGenreRequestDTO
    {
        public IOutputCacheStore OutputCacheStore { get; set; } = null!;
        public IGenresRepository GenresRepository { get; set; } = null!;
        public IMapper Mapper { get; set; } = null!;
    }
}
