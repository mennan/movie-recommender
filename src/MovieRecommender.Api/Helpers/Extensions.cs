using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieRecommender.Api.Models;
using MovieRecommender.DTO.Models;
using MovieRecommenderService.Contracts;
using Newtonsoft.Json;

namespace MovieRecommender.Api.Helpers
{
    public static class Extensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Domain"];
                options.Audience = configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        if (!(context.SecurityToken is JwtSecurityToken accessToken)) return;
                        if (context.Principal.Identity is ClaimsIdentity identity)
                        {
                            var token = accessToken.RawData;
                            await SetUserInfo(options, token, context, identity);
                        }
                    }
                };
            });
        }

        private static async Task SetUserInfo(JwtBearerOptions options, string token, TokenValidatedContext context,
            ClaimsIdentity identity)
        {
            var userInfoUrl = $"{options.Authority}userinfo";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(userInfoUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseData))
                {
                    var userService = context.HttpContext.RequestServices.GetService<IUserService>();
                    var userInfoData = JsonConvert.DeserializeObject<Auth0UserInfoModel>(responseData);
                    var userId = userService.CreateIfNotExist(new UserDto
                    {
                        UserId = userInfoData.Sub,
                        Name = userInfoData.Name,
                        NickName = userInfoData.Nickname,
                        Email = userInfoData.Email,
                        CreatedDate = DateTime.UtcNow
                    });

                    identity.AddClaim(new Claim("UserId", userId.Data.ToString()));
                }
            }
        }
    }
}