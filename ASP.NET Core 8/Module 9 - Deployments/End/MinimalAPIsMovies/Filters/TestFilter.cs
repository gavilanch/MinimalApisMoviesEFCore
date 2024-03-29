
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Filters
{
    public class TestFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, 
            EndpointFilterDelegate next)
        {
            // This is the code that will execute before the endpoint

            var param1 = context.Arguments.OfType<int>().FirstOrDefault();
            var param2 = context.Arguments.OfType<IGenresRepository>().FirstOrDefault();
            var param3 = context.Arguments.OfType<IMapper>().FirstOrDefault();

            var result = await next(context);
            // This is the code the will execute after the endpoint
            return result;
        }
    }
}
