using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieRecommender.Api.Helpers;
using MovieRecommender.Api.Models;
using MovieRecommender.DTO.Models;
using MovieRecommenderService.Contracts;

namespace MovieRecommender.Api.Controllers
{
    /// <summary>
    /// Movie operations endpoint
    /// </summary>
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;
        private readonly IMailService _mailService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService movieService, IMailService mailService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _mailService = mailService;
            _logger = logger;
        }

        /// <summary>
        /// Get movie list by paging
        /// </summary>
        /// <param name="page">Current page number</param>
        /// <param name="perPage">Per page item count</param>
        /// <returns>Movie list</returns>
        /// <response code="200">Returns movie list</response>
        /// <response code="400">If connection problem</response>
        /// <response code="500">If connection problem</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiData<List<MovieDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status500InternalServerError)]
        public IActionResult Get([FromQuery] int page = 1, [FromQuery(Name = "per_page")] int perPage = 100)
        {
            try
            {
                var movieData = _movieService.GetAll(page, perPage);

                if (movieData.Success)
                {
                    return SuccessPaged(movieData.Data.Movies, page, perPage, movieData.Data.TotalPages,
                        "Movies listed successfully.");
                }
                else
                {
                    return BadRequest<object>(null, movieData.Message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message, null);
            }
        }

        /// <summary>
        /// Get movie details by movie id
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <returns>Movie details</returns>
        /// <response code="200">Returns movie details</response>
        /// <response code="400">If connection problem</response>
        /// <response code="500">If connection problem</response>
        [HttpGet("{id}/Details")]
        [ProducesResponseType(typeof(ApiData<MovieDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status500InternalServerError)]
        public IActionResult GetDetails([FromRoute] Guid id)
        {
            try
            {
                var details = _movieService.GetDetails(id, UserId);

                return details.Success
                    ? Success(details.Data, string.Empty)
                    : BadRequest<object>(null, details.Message);
            }
            catch (Exception ex)
            {
                return Error(ex.Message, null);
            }
        }

        /// <summary>
        /// Rate any movie
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <param name="model">Rate and note informations</param>
        /// <response code="200">Returns message</response>
        /// <response code="400">If rate save problem or invalid model</response>
        /// <response code="500">If connection problem</response>
        [ValidateModel]
        [HttpPost("{id}/rate")]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status500InternalServerError)]
        public IActionResult Rate([FromRoute] Guid id, [FromBody] UserMovieRateModel model)
        {
            try
            {
                var rateData = new UserRateDto
                {
                    MovieId = id,
                    UserId = UserId,
                    Rate = model.Rate,
                    Note = model.Note
                };

                var rateResponse = _movieService.SetUserRate(rateData);

                return rateResponse.Success
                    ? Success<object>(null, rateResponse.Message)
                    : BadRequest<object>(null, rateResponse.Message);
            }
            catch (Exception ex)
            {
                return Error(ex.Message, null);
            }
        }

        /// <summary>
        /// Suggest any movie by e-mail
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <param name="model">E-mail information</param>
        /// <response code="200">Returns message</response>
        /// <response code="400">If invalid model</response>
        /// <response code="500">If connection or send e-mail problem</response>
        [ValidateModel]
        [HttpPost("{id}/suggest")]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status500InternalServerError)]
        public IActionResult Suggest([FromRoute] Guid id, [FromBody] MovieSuggestModel model)
        {
            try
            {
                var data = new MovieSuggestDto
                {
                    MovieId = id,
                    UserId = UserId,
                    Email = model.Email
                };

                _movieService.SuggestMovie(data);

                return Success<object>(null, "Movie suggested successfully.");
            }
            catch (Exception ex)
            {
                return Error(ex.Message, null);
            }
        }
    }
}