namespace MovieRecommender.Api.Models
{
    /// <summary>
    /// Generated Token Information
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// JWT Access Token
        /// </summary>
        public string AccessToken { get; set; }
        
        /// <summary>
        /// Token Expiration Information
        /// </summary>
        public int ExpiresIn { get; set; }
        
        /// <summary>
        /// Token Type
        /// </summary>
        public string TokenType { get; set; }
    }
}