using System;
using System.Collections;
using System.Collections.Generic;
using MovieRecommender.DTO.Models;
using MovieRecommenderService;

namespace MovieRecommender.Api.Tests.Stubs
{
    public class GetFailStubs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ServiceResponse<PagedMovieDto>
                {
                    Success = false,
                    Message = "Something went wrong!"
                },
                "Something went wrong!"
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}