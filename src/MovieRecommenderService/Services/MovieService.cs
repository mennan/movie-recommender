using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MovieRecommender.DTO.Models;
using MovieRecommender.Entity.Models;
using MovieRecommender.Repository;
using MovieRecommender.Repository.Contracts;
using MovieRecommenderService.Contracts;
using static MovieRecommenderService.Mappers;

namespace MovieRecommenderService.Services
{
    public class MovieService : IMovieService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public MovieService(IUnitOfWork unitOfWork, IUserService userService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mailService = mailService;
        }

        public ServiceResponse<PagedMovieDto> GetAll(int page, int perPage)
        {
            var startIndex = (page - 1) * perPage;
            var dbData = _unitOfWork.MovieRepository.FindAll();
            var count = dbData.Count();
            var movies = dbData.Skip(startIndex).Take(perPage).ToList();
            var data = Maps().Map<List<MovieDto>>(movies);
            var totalPages = Convert.ToInt32(Math.Ceiling((double) count / perPage));
            var returnData = new PagedMovieDto
            {
                Movies = data,
                TotalPages = totalPages
            };

            return new ServiceResponse<PagedMovieDto>
            {
                Success = true,
                Data = returnData
            };
        }

        public ServiceResponse<MovieInfoDto> GetDetails(Guid movieId, Guid userId)
        {
            var movie = _unitOfWork.MovieRepository.FirstOrDefault(x => x.Id == movieId);

            if (movie == null)
            {
                return new ServiceResponse<MovieInfoDto>
                {
                    Success = false,
                    Message = "Movie not found!"
                };
            }

            var ratings = _unitOfWork.UserMovieRateRepository.Find(x => x.MovieId == movieId).ToList();
            var userMovieDetails = ratings.FirstOrDefault(x => x.UserId == userId);
            var totalRatingsCount = ratings.Count;
            var averageRatingsSum = ratings.Sum(x => x.Rating);
            var averageRatings = 0d;

            if (totalRatingsCount > 0) averageRatings = (double) averageRatingsSum / (double) totalRatingsCount;

            var returnData = new MovieInfoDto
            {
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                ReleaseDate = movie.ReleaseDate,
                Rate = userMovieDetails?.Rating ?? 0,
                Note = userMovieDetails?.Note,
                AverageRate = Math.Round(averageRatings, 2)
            };

            return new ServiceResponse<MovieInfoDto>
            {
                Success = true,
                Data = returnData
            };
        }

        public ServiceResponse<bool> SetUserRate(UserRateDto rateData)
        {
            if (rateData == null) return new ServiceResponse<bool> {Success = false, Message = "Data is empty!"};

            var movie = _unitOfWork.MovieRepository.FirstOrDefault(x => x.Id == rateData.MovieId);

            if (movie == null) return new ServiceResponse<bool> {Success = false, Message = "Movie not found!"};

            var user = _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == rateData.UserId);

            if (user == null) return new ServiceResponse<bool> {Success = false, Message = "User not found!"};

            var userMovieRating = _unitOfWork.UserMovieRateRepository.FirstOrDefault(x =>
                x.MovieId == rateData.MovieId && x.UserId == rateData.UserId);

            if (userMovieRating == null)
            {
                userMovieRating = new UserMovieRate
                {
                    MovieId = rateData.MovieId,
                    UserId = rateData.UserId,
                    Rating = rateData.Rate,
                    Note = rateData.Note
                };

                _unitOfWork.UserMovieRateRepository.Add(userMovieRating);
            }
            else
            {
                userMovieRating.Rating = rateData.Rate;
                userMovieRating.Note = rateData.Note;
            }

            _unitOfWork.SaveChanges();

            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "User rating and notes saved successfully."
            };
        }

        public ServiceResponse<bool> SuggestMovie(MovieSuggestDto data)
        {
            if (data == null) return new ServiceResponse<bool> {Success = false, Message = "Data is empty!"};

            var movie = _unitOfWork.MovieRepository.FirstOrDefault(x => x.Id == data.MovieId);

            if (movie == null) return new ServiceResponse<bool> {Success = false, Message = "Movie not found!"};

            var user = _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == data.UserId);

            if (user == null) return new ServiceResponse<bool> {Success = false, Message = "User not found!"};

            _mailService.Send(new MailDto
            {
                From = "noreply@example.com",
                To = new List<string> {data.Email},
                Subject = "Movie suggestion",
                Content = $"User suggested {movie.Title} movie."
            });

            return new ServiceResponse<bool> {Success = true};
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}