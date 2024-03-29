using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public class CommentsRepository(ApplicationDbContext context) : ICommentsRepository
    {
        public async Task<List<Comment>> GetAll(int movieId)
        {
            return await context.Comments.Where(c => c.MovieId == movieId).ToListAsync();
        }

        public async Task<Comment?> GetById(int id)
        {
            return await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Create(Comment comment)
        {
            context.Add(comment);
            await context.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task Update(Comment comment)
        {
            context.Update(comment);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Comments.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}
