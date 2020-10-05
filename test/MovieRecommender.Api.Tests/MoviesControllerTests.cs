using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieRecommender.Api.Controllers;
using MovieRecommender.Api.Models;
using MovieRecommender.Api.Tests.Stubs;
using MovieRecommender.DTO.Models;
using MovieRecommenderService;
using MovieRecommenderService.Contracts;
using Xunit;

namespace MovieRecommender.Api.Tests
{
    public class MoviesControllerTests
    {
        [Theory]
        [ClassData(typeof(GetSuccessStubs))]
        public void Get_List_ReturnsMovieList(ServiceResponse<PagedMovieDto> obj, bool success, int count)
        {
            // Arrange
            var page = 1;
            var perPage = 100;
            var logger = new Mock<ILogger<MoviesController>>();
            var movieService = new Mock<IMovieService>();
            movieService.Setup(x => x.GetAll(page, perPage)).Returns(obj);

            var controller = new MoviesController(movieService.Object, logger.Object);

            // Act
            var result = controller.Get(page, perPage);

            // Assert
            var apiResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<PagedApiData<MovieDto>>(apiResult.Value);

            if (success)
                Assert.True(model.Success);
            else
                Assert.False(model.Success);

            Assert.Equal(count, model.Data.Count);
            Assert.Equal(StatusCodes.Status200OK, apiResult.StatusCode);
        }
        
        [Theory]
        [ClassData(typeof(GetFailStubs))]
        public void Get_List_ReturnsErrorMessage(ServiceResponse<PagedMovieDto> obj, string message)
        {
            // Arrange
            var page = 1;
            var perPage = 100;
            var logger = new Mock<ILogger<MoviesController>>();
            var movieService = new Mock<IMovieService>();
            movieService.Setup(x => x.GetAll(page, perPage)).Returns(obj);

            var controller = new MoviesController(movieService.Object, logger.Object);

            // Act
            var result = controller.Get(page, perPage);

            // Assert
            var apiResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<ApiData<object>>(apiResult.Value);

            Assert.Null(model.Data);
            Assert.False(model.Success);
            Assert.Equal(message, model.Message);
            Assert.Equal(StatusCodes.Status400BadRequest, apiResult.StatusCode);
        }

        [Theory]
        [ClassData(typeof(GetDetailsSuccessStubs))]
        public void GetDetails_List_ReturnsMovieDetails(ServiceResponse<MovieInfoDto> obj, double averageRate, string movieName)
        {
            // Arrange
            var movieId = It.IsAny<Guid>();
            var userId = It.IsAny<Guid>();
            var logger = new Mock<ILogger<MoviesController>>();
            var movieService = new Mock<IMovieService>();
            movieService.Setup(x => x.GetDetails(movieId, userId)).Returns(obj);

            var controller = new MoviesController(movieService.Object, logger.Object);

            // Act
            var result = controller.GetDetails(movieId);

            // Assert
            var apiResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<ApiData<MovieInfoDto>>(apiResult.Value);

            Assert.NotNull(model.Data);
            Assert.True(model.Success);
            Assert.Equal(averageRate, model.Data.AverageRate);
            Assert.Equal(movieName, model.Data.Title);
            Assert.Equal(StatusCodes.Status200OK, apiResult.StatusCode);
        }
    }
}