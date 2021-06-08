using CrudAPIWithRepositoryPattern.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using CrudAPIWithRepositoryPattern.IRepositories;

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork iUnitOfWork;


        private readonly IConfiguration _configuration;
        public AuthController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork iUnitOfWork
            )
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.roleManager = roleManager;
            this.iUnitOfWork = iUnitOfWork;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            var userExists = await userManager.FindByEmailAsync(registerViewModel.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Email already exists!" });
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerViewModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = registerViewModel.Name,
                UserName = registerViewModel.Email
            };

            var role = await roleManager.RoleExistsAsync("Student");
            if (!role)
            {
                var newRole = new IdentityRole() { Name = "Student" };
                await roleManager.CreateAsync(newRole);
            }

            var result = await userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Errors = result.Errors, Message = "User creation failed! Please check user details and try again." });
            }
            await userManager.AddToRoleAsync(user, "Student");
            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(2),//AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var refreshToken = Guid.NewGuid().ToString().Replace('-', '.');
                var expiry = DateTime.Now.AddHours(1);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDate = expiry;
                iUnitOfWork.SaveChanges();


                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userName = user.Name,
                    refreshToken = refreshToken
                });
            }
            return Unauthorized();
        }


        [HttpPost, Route("checkEmailAvailability")]
        public async Task<IActionResult> CheckEmailAddress([FromBody] LoginViewModel model)
        {
            var result = await userManager.FindByEmailAsync(model.Email);
            if (result == null) return Ok(new { message = false });
            return Ok(new { message = true });
        }
        [HttpPost, Route("refreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenViewModel tokens)
        {
            if (string.IsNullOrEmpty(tokens.RefreshToken) || string.IsNullOrEmpty(tokens.Token))
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            SecurityToken securityToken;


            var principal = tokenHandler.ValidateToken(tokens.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            }, out securityToken) ;

            var jwtToken = securityToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }
            var newToken = new JwtSecurityToken(
              issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(2),// DateTime.Now.AddHours(3),
                    claims: principal.Claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(newToken), refreshToken = Guid.NewGuid().ToString().Replace('-', '.') });
        
        }

    }
}
