using System;

namespace MovieRecommender.DTO.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}