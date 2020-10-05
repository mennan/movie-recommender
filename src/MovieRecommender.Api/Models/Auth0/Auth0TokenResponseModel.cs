using Newtonsoft.Json;

namespace MovieRecommender.Api.Models
{
    public class Auth0TokenResponseModel
    {
        [JsonProperty("access_token")] 
        public string AccessToken { get; set; }
        
        [JsonProperty("id_token")] 
        public string IdToken { get; set; }

        public string Scope { get; set; }
        
        [JsonProperty("expires_in")] 
        public int ExpiresIn { get; set; }
        
        [JsonProperty("token_type")] 
        public string TokenType { get; set; }
    }
}