using System;
using System.Collections;
using System.Collections.Generic;
using MovieRecommender.DTO.Models;
using MovieRecommenderService;

namespace MovieRecommender.Api.Tests.Stubs
{
    public class GetDetailsSuccessStubs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ServiceResponse<MovieInfoDto>
                {
                    Success = true,
                    Data = new MovieInfoDto
                    {
                        Note = "Great movie",
                        Rate = 9,
                        AverageRate = 7.8,
                        Title = "Matrix",
                        OriginalTitle = "Matrix",
                        ReleaseDate = new DateTime(1999, 9, 3)
                    },
                    Message = "Movie details listed successfully."
                },
                7.8d,
                "Matrix"
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}