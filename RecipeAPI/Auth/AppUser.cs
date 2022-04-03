using Microsoft.AspNetCore.Identity;
using System;

namespace RecipeAPI.Auth
{
    public class AppUser : IdentityUser<int>
    {
        public int? Age { get; set; }
        public string City { get; set; }
        public string JWTRefreshToken { get; set; }
        public DateTime? JWTRefreshTokenEndDate { get; set; }
    }
}
