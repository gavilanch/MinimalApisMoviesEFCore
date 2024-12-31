using MinimalAPIsMovies.Entities;
using Error = MinimalAPIsMovies.Entities.Error;

namespace MinimalAPIsMovies.Repositories
{
    public interface IErrorsRepository
    {
        Task Create(Error error);
    }
}