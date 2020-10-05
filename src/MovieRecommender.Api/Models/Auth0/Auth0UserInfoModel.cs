using System;
using Newtonsoft.Json;

namespace MovieRecommender.Api.Models
{
    public class Auth0UserInfoModel
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public Uri Picture { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }
    }
}