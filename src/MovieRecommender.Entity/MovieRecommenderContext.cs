using Microsoft.EntityFrameworkCore;
using MovieRecommender.Entity.Models;

namespace MovieRecommender.Entity
{
    public class MovieRecommenderContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovieRate> UserMovieRates { get; set; }
        public DbSet<User> Users { get; set; }

        public MovieRecommenderContext(DbContextOptions<MovieRecommenderContext> options) : base(options)
        {
        }
    }
}