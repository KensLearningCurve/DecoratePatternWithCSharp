﻿using CachingDemo.Business.Entities;
using CachingDemo.Business.Interfaces;

namespace CachingDemo.Business
{
    public class MovieService : IMovieService
    {
        private readonly DataContext _dbContext;

        public MovieService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Movie movie)
        {
            _dbContext.Set<Movie>().Add(movie);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Movie? toDelete = Get(id);

            if (toDelete == null)
                return;

            _dbContext.Remove(toDelete);
            _dbContext.SaveChanges();
        }

        public Movie? Get(int id) => _dbContext.Set<Movie>().FirstOrDefault(x => x.Id == id);

        public IEnumerable<Movie> GetAll() => _dbContext.Set<Movie>().ToList();
    }
}
