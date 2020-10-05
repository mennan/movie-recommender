using System;
using System.Collections.Generic;
using MovieRecommender.Entity;
using MovieRecommender.Entity.Models;
using MovieRecommender.Repository.Contracts;

namespace MovieRecommender.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieRecommenderContext _context;
        private bool disposed;
        private IRepository<Movie> _movieRepository;
        private IRepository<User> _userRepository;
        private IRepository<UserMovieRate> _userMovieRateRepository;

        public IRepository<Movie> MovieRepository => _movieRepository ??= new Repository<Movie>(_context);
        public IRepository<User> UserRepository => _userRepository ??= new Repository<User>(_context);

        public IRepository<UserMovieRate> UserMovieRateRepository =>
            _userMovieRateRepository ??= new Repository<UserMovieRate>(_context);

        public UnitOfWork(MovieRecommenderContext context)
        {
            this._context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}