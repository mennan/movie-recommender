using System.ComponentModel.DataAnnotations;

namespace MovieRecommender.Api.Models
{
    /// <summary>
    /// User rate information
    /// </summary>
    public class UserMovieRateModel
    {
        /// <summary>
        /// User rate
        /// </summary>
        [Required]
        [Range(1, 10)]
        public short Rate { get; set; }
        
        /// <summary>
        /// User notes
        /// </summary>
        [Required]
        public string Note { get; set; }
    }
}