using System;
using System.Collections;
using System.Collections.Generic;
using MovieRecommender.DTO.Models;
using MovieRecommenderService;

namespace MovieRecommender.Api.Tests.Stubs
{
    public class GetSuccessStubs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ServiceResponse<PagedMovieDto>
                {
                    Success = true,
                    Data = new PagedMovieDto
                    {
                        Movies = new List<MovieDto>
                        {
                            new MovieDto
                            {
                                Title = "Matrix",
                                OriginalTitle = "Matrix",
                                ReleaseDate = new DateTime(1999, 9, 3)
                            }
                        },
                        TotalPages = 5
                    }
                },
                true, 1
            };

            yield return new object[]
            {
                new ServiceResponse<PagedMovieDto>
                {
                    Success = true,
                    Data = new PagedMovieDto
                    {
                        Movies = new List<MovieDto>
                        {
                            new MovieDto
                            {
                                Title = "Matrix",
                                OriginalTitle = "Matrix",
                                ReleaseDate = new DateTime(1999, 9, 3)
                            },
                            new MovieDto
                            {
                                Title = "The Lord of the Rings",
                                OriginalTitle = "Matrix",
                                ReleaseDate = new DateTime(2001, 12, 21)
                            }
                        },
                        TotalPages = 7
                    }
                },
                true, 2
            };

            yield return new object[]
            {
                new ServiceResponse<PagedMovieDto>
                {
                    Success = true,
                    Data = new PagedMovieDto
                    {
                        Movies = new List<MovieDto>(),
                        TotalPages = 1
                    }
                },
                true, 0
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}