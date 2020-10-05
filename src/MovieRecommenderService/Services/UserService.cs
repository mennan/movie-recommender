using System;
using MovieRecommender.DTO.Models;
using MovieRecommender.Entity.Models;
using MovieRecommender.Repository.Contracts;
using MovieRecommenderService.Contracts;
using static MovieRecommenderService.Mappers;

namespace MovieRecommenderService.Services
{
    public class UserService : IUserService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse<Guid> CreateIfNotExist(UserDto user)
        {
            var userInfo = GetUser(user.Email);

            if (userInfo.Data != null) return new ServiceResponse<Guid> {Success = true, Data = userInfo.Data.Id};

            var data = Maps().Map<User>(user);
            data.Id = Guid.NewGuid();

            _unitOfWork.UserRepository.Add(data);
            _unitOfWork.SaveChanges();

            return new ServiceResponse<Guid> {Success = true, Data = data.Id};
        }

        public ServiceResponse<UserDto> GetUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = _unitOfWork.UserRepository.FirstOrDefault(x => x.Email == email);
            var returnData = Maps().Map<UserDto>(user);

            return new ServiceResponse<UserDto>
            {
                Success = true,
                Data = returnData
            };
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}