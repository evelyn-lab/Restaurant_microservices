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
    // Атрибут указывает маршрут, по которому будет доступен контроллер.
    [Route("api/[controller]")]
    // Атрибут указывает, что данный контроллер является контроллером API
    [ApiController]

    // Класс контроллера входа пользователя в систему
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        // Контекст данных для взаимодействия с базой данных
        private readonly DataContext _context;
        
        // Метод хеширования пароля пользователя с использованием алгоритма MD5
        private static string PasswordHash(string password)
        {
            using var hashAlg = MD5.Create();
            byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder(hash.Length * 2);
            foreach (var t in hash)
            {
                builder.Append(t.ToString("X2"));
            }
            return builder.ToString();
        }
        
        public LoginController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        // Метод, обрабатывающий HTTP POST-запрос для входа пользователя
        [HttpPost]
        public async Task<ActionResult<Session>> LoginProcess([FromBody] Login login)
        {
            var user = Authentication(login);
            if (user != null)
            {
                string token = Generate(user);
                Session currentSession = new()
                {
                    UserId = user.Id,
                    SessionToken = token,
                    ExpiresAt = DateTime.Now.AddDays(1)
                };
                _context.Session!.Add(currentSession);
                await _context.SaveChangesAsync();
                return Ok("Authorization success. Token: " + token);
            }
            return NotFound("User not found");
        }
        
        /*
         * Метод для генерации JWT-токена на основе роли пользователя.
         * Создает и подписывает токен с помощью класса JwtSecurityTokenHandler и возвращает его в виде строки.
         */
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role!)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            string result = new JwtSecurityTokenHandler().WriteToken(token);
            return result;
        }

        /*
         * Метод для аутентификации пользователя.
         * Проверяет соответствие введенных пользователем данных с данными в базе данных.
         */
        private User? Authentication(Login login)
        {
            User? currentUser = null;
            foreach (User user in _context.User!)
            {
                if (user.Email!.ToLower() == login.Email!.ToLower() && user.Password == PasswordHash(login.Password!))
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