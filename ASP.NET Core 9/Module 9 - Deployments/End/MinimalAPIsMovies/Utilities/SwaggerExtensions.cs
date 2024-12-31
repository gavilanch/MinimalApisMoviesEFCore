using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Utilities
{
    public static class SwaggerExtensions
    {
        public static TBuilder AddMoviesFilterParameters<TBuilder>(this TBuilder builder)
            where TBuilder: IEndpointConventionBuilder
        {
            return builder.WithOpenApi(options =>
            {
                AddPaginationParameters(options);

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Title",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "InTheaters",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean"
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "GenreId",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer"
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "FutureReleases",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean"
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "OrderByField",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new List<IOpenApiAny> { 
                            new OpenApiString("Title"), 
                            new OpenApiString("ReleaseDate")
                        }
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "OrderByAscending",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean"
                    }
                });


                return options;
            });
        }

        public static TBuilder AddPaginationParameters<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(options =>
            {
                AddPaginationParameters(options);
                return options;
            });
        }

        private static void AddPaginationParameters(OpenApiOperation openApiOperation)
        {
            openApiOperation.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
            {
                Name = "Page",
                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "integer",
                    Default = new OpenApiInteger(PaginationDTO.pageInitialValue)
                }
            });

            openApiOperation.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
            {
                Name = "RecordsPerPage",
                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "integer",
                    Default = new OpenApiInteger(PaginationDTO.recordsPerPageInitialValue)
                }
            });
        }
    }
}
