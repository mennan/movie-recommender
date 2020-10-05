using System;
using MovieRecommender.DTO.Models;

namespace MovieRecommenderService.Contracts
{
    public interface IUserService
    {
        ServiceResponse<Guid> CreateIfNotExist(UserDto user);
        ServiceResponse<UserDto> GetUser(string email);
    }
}