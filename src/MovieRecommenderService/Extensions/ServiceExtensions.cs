using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieRecommender.Entity;
using MovieRecommender.Repository;
using MovieRecommender.Repository.Contracts;
using MovieRecommenderService.Contracts;
using MovieRecommenderService.Services;

namespace MovieRecommenderService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Db");
            services.AddDbContext<MovieRecommenderContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static void ConfigureRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailService>(x => new MailService
            {
                Host = configuration["Mail:Host"],
                Port = string.IsNullOrEmpty(configuration["Mail:Port"])
                    ? 0
                    : Convert.ToInt32(configuration["Mail:Port"]),
                UserName = configuration["Mail:UserName"],
                Password = configuration["Mail:Password"],
                UseSsl = !string.IsNullOrEmpty(configuration["Mail:UseSsl"]) &&
                         Convert.ToBoolean(configuration["Mail:UseSsl"])
            });
        }
    }
}