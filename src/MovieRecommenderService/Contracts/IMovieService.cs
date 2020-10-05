using System;
using MovieRecommender.DTO.Models;

namespace MovieRecommenderService.Contracts
{
    public interface IMovieService
    {
        ServiceResponse<PagedMovieDto> GetAll(int page, int perPage);
        ServiceResponse<MovieInfoDto> GetDetails(Guid movieId, Guid userId);
        ServiceResponse<bool> SetUserRate(UserRateDto rateData);
        ServiceResponse<bool> SuggestMovie(MovieSuggestDto data);
    }
}