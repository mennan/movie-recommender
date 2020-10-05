using Newtonsoft.Json;

namespace MovieRecommender.Api.Models
{
    public class Auth0TokenRequestModel
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}