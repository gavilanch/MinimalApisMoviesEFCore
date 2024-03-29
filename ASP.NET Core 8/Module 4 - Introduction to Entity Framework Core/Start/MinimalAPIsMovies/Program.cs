using Microsoft.AspNetCore.Cors;
using MinimalAPIsMovies.Entities;

var builder = WebApplication.CreateBuilder(args);

// Services zone - BEGIN

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!).AllowAnyMethod()
        .AllowAnyHeader();
    });

    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services zone - END

var app = builder.Build();
// Middlewares zone - BEGIN

//if (builder.Environment.IsDevelopment())
//{
   
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseOutputCache();

app.MapGet("/", () => "Hello, world");

app.MapGet("/genres", () =>
{
    var genres = new List<Genre>()
    {
        new Genre
        {
            Id = 1,
            Name = "Drama"
        },
        new Genre
        {
            Id = 2,
            Name = "Action"
        },
        new Genre
        {
            Id = 3,
            Name = "Comedy"
        }
    };

    return genres;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

// Middlewares zone - END
app.Run();
