using System;

namespace RecipeAPI.Auth
{
    public class JWTToken
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
