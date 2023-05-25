using KPO_hw.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using KPO_hw.Context;

namespace KPO_hw.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class RegistrationController : ControllerBase
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
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public RegistrationController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;

        }
        [HttpPost]
        public async Task<ActionResult<User>> Reg(Registration reg)
        {
            if (reg.Role != "customer" && reg.Role != "chef" && reg.Role != "manager")
            {
                return Problem("Non-existent role");
            }

            if (!IsValidEmail(reg.Email))
            {
                return Problem("Wrong email address");
            }
            foreach (User u in _context.User)
            {
                if (u.Email == reg.Email)
                {
                    return Problem("This email has already been registered");
                }
                if (u.UserName == reg.UserName)
                {
                    return Problem("This username has already been registered");
                }
            }
            string passwordHash = PasswordHash(reg.Password);
            DateTime createdAt = DateTime.Now;
            User user = new User();
            user.UserName = reg.UserName;
            user.Password = PasswordHash(reg.Password);
            user.Email = reg.Email;
            user.Role = reg.Role;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Registration success");
        }

    }
}