using System;

namespace MovieRecommender.DTO.Models
{
    public class MovieInfoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AverageRate { get; set; }
        public int Rate { get; set; }
        public string Note { get; set; }
    }
}