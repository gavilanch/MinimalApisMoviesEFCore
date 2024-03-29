using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public class ActorsRepository(ApplicationDbContext context, 
        IHttpContextAccessor httpContextAccessor) : IActorsRepository
    {
        public async Task<List<Actor>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Actors.AsQueryable();
            await httpContextAccessor
                .HttpContext!.InsertPaginationParameterInResponseHeader(queryable);
            return await queryable.OrderBy(a => a.Name).Paginate(pagination).ToListAsync();
        }

        public async Task<Actor?> GetById(int id)
        {
            return await context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Actor>> GetByName(string name)
        {
            return await context.Actors
                .Where(a => a.Name.Contains(name))
                .OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<int> Create(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> Exist(int id)
        {
            return await context.Actors.AnyAsync(a => a.Id == id);
        }

        public async Task<List<int>> Exists(List<int> ids)
        {
            return await context.Actors.Where(a => ids.Contains(a.Id)).Select(a => a.Id)
                .ToListAsync();
        }

        public async Task Update(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Actors.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
