using FakeShop.Data.Interfaces;
using FakeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FakeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher<UserDocument> hasher;
        private readonly IUserRepository userRepository;

        public AuthController(
            IConfiguration configuration,
            IPasswordHasher<UserDocument> hasher,
            IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.hasher = hasher;
            this.userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            var user = await userRepository.GetAsync(model.Email);
            if(!ModelState.IsValid || user == null)
            {
                return Unauthorized();
            }

            var test = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (test != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            var subject = new ClaimsIdentity(new[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                new Claim(JwtRegisteredClaimNames.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId)
            });

            var expires = DateTime.UtcNow.AddMinutes(10);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(jwtToken);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = await userRepository.GetAsync(model.Email);
            if (ModelState.IsValid && user == null)
            {
                var id = Guid.NewGuid().ToString();
                user = new UserDocument()
                {
                    Email = model.Email,
                    UserId = id,
                    Name = model.Name,
                    PasswordHash = hasher.HashPassword(user, model.Password)
                };

                await userRepository.AddAsync(user);
                return Ok();
            }

            return BadRequest("User with that email is already registered.");
        }
    }
}
