using System;

namespace MovieRecommender.DTO.Models
{
    public class MovieSuggestDto
    {
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}