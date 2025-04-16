using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
                return BadRequest("Phone number is already taken.");

            string hashedPassword = ComputeSha256Hash(model.Password);
            var user = new User
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Role = model.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            User user = null;

            if (model.Identifier.Contains("@"))
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Identifier);
            else
                user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.Identifier);

            if (user == null || !VerifySha256Hash(model.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);

            return Ok(token);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifySha256Hash(string input, string storedHash)
        {
            string hashOfInput = ComputeSha256Hash(input);
            return hashOfInput.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretKey);

            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated JWT Token: {jwtToken}");

            return jwtToken;
        }

        [Authorize]
        [HttpGet("authe")]
        public async Task<IActionResult> authe()
        {
            

            return Ok("you are authe");
        }

    }
}