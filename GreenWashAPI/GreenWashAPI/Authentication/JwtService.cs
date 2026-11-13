using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GreenWashAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GreenWashAPI.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        // Static key to persist the randomly generated secret during the application lifetime.
        private static string _secretKey;

        public string SecretKey => _secretKey;
        public JwtService(IConfiguration config)
        {
            _config = config;
            // Check if key exists in configuration; if not, generate a random one.
            var keyFromConfig = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyFromConfig))
            {
                _secretKey = GenerateRandomKey(32); // 256-bit key (32 bytes)
            }
            else
            {
                _secretKey = keyFromConfig;
            }
        }
        // Generate a Base64 encoded random key of the specified length (in bytes)
        private string GenerateRandomKey(int length)
        {
            var key = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.EmailId),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
















