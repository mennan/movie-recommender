using System.Collections.Generic;

namespace MovieRecommender.DTO.Models
{
    public class MailDto
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<string> To { get; set; } = new List<string>();
        public string From { get; set; }
    }
}