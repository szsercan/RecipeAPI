using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecipeAPI.Auth
{
    public class JWTTokenManager
    {
        readonly IConfiguration _configuration;

        public JWTTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JWTToken CreateAccessToken(AppUser user, List<Claim> claims)
        {
            JWTToken tokenInstance = new JWTToken();
            tokenInstance.UserName = user.UserName;
            tokenInstance.Expiration = DateTime.Now.AddMinutes(2);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: tokenInstance.Expiration,
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            tokenInstance.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            tokenInstance.RefreshToken = CreateRefreshToken();

            return tokenInstance;
        }

        private string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}
