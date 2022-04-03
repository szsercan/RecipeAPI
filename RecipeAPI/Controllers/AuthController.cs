using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RecipeAPI.Auth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserManager<AppUser> _userManager;
        RoleManager<AppRole> _roleManager;
        IConfiguration _configuration;
        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            AppUser appUser = new AppUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(appUser, registerViewModel.Parola);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(registerViewModel.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, registerViewModel.Parola))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var JWTToken = new JWTTokenManager(_configuration).CreateAccessToken(user, authClaims);

                    user.JWTRefreshToken = JWTToken.RefreshToken;
                    user.JWTRefreshTokenEndDate = JWTToken.Expiration.AddMinutes(1);
                    var resultUpdate = await _userManager.UpdateAsync(user);
                    if (!resultUpdate.Succeeded)
                        return BadRequest();

                    return Ok(JWTToken);
                }
                return BadRequest();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] RegisterViewModel registerViewModel)
        {
            var user = await _userManager.FindByNameAsync(registerViewModel.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, registerViewModel.Parola))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var JWTToken = new JWTTokenManager(_configuration).CreateAccessToken(user, authClaims);

                user.JWTRefreshToken = JWTToken.RefreshToken;
                user.JWTRefreshTokenEndDate = JWTToken.Expiration.AddMinutes(1);
                var resultUpdate = await _userManager.UpdateAsync(user);
                if (!resultUpdate.Succeeded)
                    return BadRequest();

                return Ok(JWTToken);
            }
            return BadRequest();
        }
    }


}
