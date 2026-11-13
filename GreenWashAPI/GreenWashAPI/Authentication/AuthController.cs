using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenWashAPI.Authentication;
using GreenWashAPI.Models;
using GreenWashAPI.Services;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;

namespace GreenWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserVM user)
        {
            if (string.IsNullOrEmpty(user.EmailId) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Email and password are required.");
            }

            if (!Regex.IsMatch(user.EmailId, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email format.");
            }


            User userObj= new User()
            {
                Fullname = user.Fullname,
                Address = user.Address,
                EmailId = user.EmailId,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Role = user.Role
            };
            // Check if the email is already registered.
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EmailId == userObj.EmailId);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }
            
       //     userObj.Password = BCrypt.HashPassword(userObj.Password);
            // In a production app, you would hash the password before saving.
            userObj.CreatedAt = DateTime.UtcNow;
            userObj.UpdatedAt = DateTime.UtcNow;
            userObj.IsActive = true;
            _context.Users.Add(userObj);
            await _context.SaveChangesAsync();
            return Ok("Registration successful.");
        }
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // In production, use password hashing and secure comparison.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailId == request.Email && u.Password == request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            var token = _jwtService.GenerateToken(user);
          //  return Ok(new { Token = token });
            return Ok(new { token, role = user.Role ,userId=user.UserId});
        }
    }
}