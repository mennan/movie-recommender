using System;

namespace MovieRecommender.DTO.Models
{
    public class UserRateDto
    {
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public short Rate { get; set; }
        public string Note { get; set; }
    }
}