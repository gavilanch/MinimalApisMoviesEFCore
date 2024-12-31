using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public interface IGenresRepository
    {
        Task<int> Create(Genre genre);
        Task<Genre?> GetById(int id);
        Task<List<Genre>> GetAll();
        Task<bool> Exists(int id);
        Task Update(Genre genre);
        Task Delete(int id);
    }
}
