using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovieRecommender.Api.Helpers;
using MovieRecommender.Api.Models;
using Newtonsoft.Json;

namespace MovieRecommender.Api.Controllers
{
    /// <summary>
    /// Authentication endpoint
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Generate JWT Token
        /// </summary>
        /// <param name="model">User name and password information</param>
        /// <returns></returns>
        /// <response code="200">Returns generated Auth0 JWT token</response>
        /// <response code="400">If username or password is invalid</response>   
        /// <response code="500">If connection problem</response>   
        [AllowAnonymous]
        [ValidateModel]
        [HttpPost("token")]
        [ProducesResponseType(typeof(ApiData<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiData<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Token([FromBody] LoginModel model)
        {
            var url = $"{_configuration["Auth0:Domain"]}oauth/token";

            using var client = new HttpClient();
            var data = new Auth0TokenRequestModel
            {
                Audience = _configuration["Auth0:Audience"],
                GrantType = "password",
                UserName = model.UserName,
                Password = model.Password,
                Scope = "openid",
                ClientId = _configuration["Auth0:ClientId"],
                ClientSecret = _configuration["Auth0:ClientSecret"]
            };
            var convertedData = JsonConvert.SerializeObject(data);
            var requestData = new StringContent(convertedData, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, requestData);
                var responseData = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(responseData))
                    {
                        var tokenData = JsonConvert.DeserializeObject<Auth0TokenResponseModel>(responseData);
                        var returnData = new TokenModel
                        {
                            AccessToken = tokenData.AccessToken,
                            ExpiresIn = tokenData.ExpiresIn,
                            TokenType = tokenData.TokenType
                        };

                        return Success(tokenData, "Token generated successfully.");
                    }
                }

                var errorData = JsonConvert.DeserializeObject<Auth0TokenErrorModel>(responseData);
                return BadRequest<object>(null, errorData.ErrorDescription);
            }
            catch (Exception ex)
            {
                return Error(ex.Message, null);
            }
        }
    }
}