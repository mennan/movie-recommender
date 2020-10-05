using System;
using System.Collections.Generic;

namespace MovieRecommender.DTO.Models
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class PagedMovieDto
    {
        public List<MovieDto> Movies { get; set; }
        public int TotalPages { get; set; }
    }
}