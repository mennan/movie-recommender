using AutoMapper;
using MovieRecommender.DTO.Models;
using MovieRecommender.Entity.Models;

namespace MovieRecommenderService
{
    public static class Mappers
    {
        private static readonly IMapper _maps;

        static Mappers()
        {
            if (_maps != null) return;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieDto>().ReverseMap();
                cfg.CreateMap<User, UserDto>().ReverseMap();
            });
            
            _maps = config.CreateMapper();
        }

        public static IMapper Maps()
        {
            return _maps;
        }
    }
}