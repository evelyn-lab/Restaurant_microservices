using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using KPO_hw.Context;
using KPO_hw.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace KPO_hw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private DataContext _context;
        
        private static string PasswordHash(string password)
        {
            using (var hashAlg = MD5.Create())
            {
                byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("X2"));
                }
                return builder.ToString();
            }
        }
        
        public LoginController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Session>> LoginProcess([FromBody] Login login)
        {
            var user = Authentication(login);
            if (user != null)
            {
                string token = Generate(user);
                Session currentSession = new();
                currentSession.UserId = user.Id;
                currentSession.SessionToken = token;
                currentSession.ExpiresAt = DateTime.Now.AddDays(1);
                _context.Session.Add(currentSession);
                await _context.SaveChangesAsync();
                return Ok("Authorization success. Token: " + token);
            }
            return NotFound("User not found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            string result = new JwtSecurityTokenHandler().WriteToken(token);
            return result;
        }

        private User Authentication(Login login)
        {
            User currentUser = null;
            foreach (User user in _context.User)
            {
                if (user.Email.ToLower() == login.Email.ToLower() && user.Password == PasswordHash(login.Password))
                {
                    currentUser = user;
                }
            }
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
        
        
    }
}