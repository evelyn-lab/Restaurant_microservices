using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KPO_hw.Context;
using KPO_hw.Models;

namespace KPO_hw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        // GET: api/User/token
        [HttpGet("{token}")]
        public async Task<ActionResult<User>> GetSession(string token)
        {
          if (_context.Session == null)
          {
              return NotFound();
          }
          var session = _context.Session.FirstOrDefault(s => s.SessionToken == token);
          if (session == null)
          {
              return NotFound("Token doesn't exist");
          }
          var user = _context.User.FirstOrDefault(u => u.Id == session.UserId);
          return user;
        }
    }
}
