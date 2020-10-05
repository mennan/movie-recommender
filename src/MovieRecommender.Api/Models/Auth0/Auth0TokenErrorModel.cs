using Newtonsoft.Json;

namespace MovieRecommender.Api.Models
{
    public class Auth0TokenErrorModel
    {
        public string Error { get; set; }
        
        [JsonProperty("error_description")] 
        public string ErrorDescription { get; set; }
    }
}