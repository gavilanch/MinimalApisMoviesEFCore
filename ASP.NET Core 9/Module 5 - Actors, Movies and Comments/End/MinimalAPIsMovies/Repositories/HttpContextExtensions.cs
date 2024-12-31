using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsMovies.Repositories
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameterInResponseHeader<T>(
            this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Append("totalAmountOfRecords", count.ToString());
        }
    }
}
