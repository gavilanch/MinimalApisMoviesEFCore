using Microsoft.AspNetCore.Identity;

namespace MinimalAPIsMovies.Services
{
    public interface IUsersService
    {
        Task<IdentityUser?> GetUser();
    }
}