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

namespace CrudAPIWithRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration _configuration;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.roleManager = roleManager;
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

            var role =await roleManager.RoleExistsAsync("Student");
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
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
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
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userName=user.Name
                });
            }
            return Unauthorized();
        }
        

        [HttpPost,Route("checkEmailAvailability")]
        public async Task<IActionResult> CheckEmailAddress([FromBody] LoginViewModel model)
        {
            var result = await userManager.FindByEmailAsync(model.Email);
            if (result == null) return Ok(new{ message =false});
            return Ok(new { message = true });
        }

    }
}
