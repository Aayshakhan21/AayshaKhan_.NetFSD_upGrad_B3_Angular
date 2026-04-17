using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.DTOs;
using WebApplication7.Models;
using WebApplication7.Exceptions;

namespace WebApplication7.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IConfiguration config, ILogger<AuthController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            _logger.LogInformation($"Register attempt for user: {user?.Username}");

            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Invalid user data");

            var exists = await _context.Users.AnyAsync(x => x.Username == user.Username);

            if (exists)
            {
                _logger.LogWarning($"User already exists: {user.Username}");
                throw new ArgumentException("User already exists");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User registered successfully: {user.Username}");

            return Ok(new { message = "User registered successfully" });
        }

        //  LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            _logger.LogInformation($"Login attempt for user: {dto?.Username}");

            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Invalid login request");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username && x.Password == dto.Password);

            if (user == null)
            {
                _logger.LogWarning($"Invalid login attempt for user: {dto.Username}");
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var keyString = _config["Jwt:Key"] ?? throw new Exception("JWT Key missing");
            var issuer = _config["Jwt:Issuer"] ?? throw new Exception("JWT Issuer missing");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation($"User logged in successfully: {user.Username}");

            return Ok(new
            {
                token = tokenString,
                username = user.Username,
                role = user.Role
            });
        }
    }
}