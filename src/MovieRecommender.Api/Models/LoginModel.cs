using System.ComponentModel.DataAnnotations;

namespace MovieRecommender.Api.Models
{
    /// <summary>
    /// Login information
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Auth0 Username
        /// </summary>
        [Required]
        public string UserName { get; set; }
        
        /// <summary>
        /// Auth0 Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}