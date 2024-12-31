
using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> 
            InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext
                .RequestServices.GetService<IValidator<T>>();

            if (validator is null)
            {
                return await next(context);
            }

            var obj = context.Arguments.OfType<T>().FirstOrDefault();

            if (obj is null)
            {
                return Results.Problem("The object to validate could not be found");
            }

            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await next(context);
        }
    }
}
