using System;
using MovieRecommender.Entity.Models;

namespace MovieRecommender.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        IRepository<Movie> MovieRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserMovieRate> UserMovieRateRepository { get; }
    }
}