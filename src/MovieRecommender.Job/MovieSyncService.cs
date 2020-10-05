using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieRecommender.Entity.Models;
using MovieRecommender.Job.Models;
using MovieRecommender.Repository.Contracts;
using Newtonsoft.Json;

namespace MovieRecommender.Job
{
    public class MovieSyncService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private const int MaxPageCount = 100;

        public MovieSyncService(ILogger<MovieSyncService> logger, IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sync service started.");

            _timer = new Timer(Sync, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sync service stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void Sync(object state)
        {
            try
            {
                var apiKey = _configuration["Tmdb:ApiKey"];
                var itemCount = 0;
                var page = 1;
                var hasItems = true;
                using var client = new HttpClient();
                using var scope = _scopeFactory.CreateScope();
                using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                while (hasItems)
                {
                    var url = string.Format(_configuration["Tmdb:ApiUrl"], apiKey, page);
                    var response = client.GetAsync(url).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode) continue;

                    var responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (responseData == null) continue;

                    page++;

                    var data = JsonConvert.DeserializeObject<TmdbMovieListModel>(responseData);

                    if (data.Results.Count == 0 || page > MaxPageCount) hasItems = false;

                    foreach (var movie in data.Results)
                    {
                        AddOrUpdateMovie(movie, uow);
                        itemCount++;
                    }

                    uow.SaveChanges();

                    _logger.LogDebug($"Page {page} synced.");
                }

                _logger.LogInformation($"Sync completed. Sync item count is {itemCount}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred on movie sync process!");
            }
        }

        private void AddOrUpdateMovie(TmdbMovieResultModel movieInfo, IUnitOfWork uow)
        {
            var movie = uow.MovieRepository.FirstOrDefault(x => x.OriginalTitle == movieInfo.OriginalTitle);
            var releaseDate = movieInfo.ReleaseDate?.Date ?? DateTime.MinValue;

            if (movie == null)
            {
                movie = new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = movieInfo.Title,
                    OriginalTitle = movieInfo.OriginalTitle,
                    ReleaseDate = releaseDate
                };

                uow.MovieRepository.Add(movie);
            }
            else
            {
                movie.Title = movieInfo.Title;
                movie.OriginalTitle = movieInfo.OriginalTitle;
                movie.ReleaseDate = releaseDate;
            }
        }
    }
}