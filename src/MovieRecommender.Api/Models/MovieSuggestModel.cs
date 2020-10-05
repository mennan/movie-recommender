using System.ComponentModel.DataAnnotations;

namespace MovieRecommender.Api.Models
{
    /// <summary>
    /// Movie Suggestion Information
    /// </summary>
    public class MovieSuggestModel
    {
        /// <summary>
        /// Email address to which the suggestion will be sent
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}