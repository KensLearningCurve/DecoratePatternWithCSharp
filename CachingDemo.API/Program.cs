using CachingDemo.Business;
using CachingDemo.Business.Entities;
using CachingDemo.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DecoratePatternDemo;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Mappings
app.MapGet("/movies", (IMovieService movieService) =>
{
    return Results.Ok(movieService.GetAll().ToList());
});

app.MapPost("/movies", (Movie movie, IMovieService movieService) =>
{
    try
    {
        movieService.Create(movie);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.MapDelete("/movies/{id:int}", (int id, IMovieService movieService) =>
{
    try
    {
        movieService.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
});
#endregion

app.Run();