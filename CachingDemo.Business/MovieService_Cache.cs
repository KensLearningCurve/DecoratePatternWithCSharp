using CachingDemo.Business.Entities;
using CachingDemo.Business.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CachingDemo.Business
{
public class MovieService_Cache : IMovieService
{
    private readonly IMemoryCache memoryCache;
    private readonly IMovieService movieService;

    public MovieService_Cache(IMemoryCache memoryCache, IMovieService movieService)
    {
        this.memoryCache = memoryCache;
        this.movieService = movieService;
    }

    public void Create(Movie movie)
    {
        movieService.Create(movie);
    }

    public void Delete(int id)
    {
        movieService.Delete(id);
    }

    public Movie? Get(int id)
    {
        string key = $"movie_{id}";

        if (memoryCache.TryGetValue(key, out Movie? movie))
            return movie;

        movie = movieService.Get(id);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(10))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

        memoryCache.Set(key, movie, cacheOptions);

        return movie;
    }

    public IEnumerable<Movie> GetAll()
    {
        string key = $"movies";

        if (memoryCache.TryGetValue(key, out List<Movie>? movies))
            return movies ?? new List<Movie>();

        movies = movieService.GetAll().ToList();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(10))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

        memoryCache.Set(key, movies, cacheOptions);

        return movies;
    }
}
}
