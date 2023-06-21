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
    // Атрибут указывает маршрут, по которому будет доступен контроллер.
    [Route("api/[controller]")]
    // Атрибут указывает, что данный контроллер является контроллером API
    [ApiController]
    
    // Класс контроллера предоставелния информации о пользователе
    public class UserController : ControllerBase
    {
        // Контекст данных для взаимодействия с базой данных
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        // Данное действие будет обрабатывать HTTP GET запросы
        // {token} - это параметр пути, который будет передавать значение токена в URL запроса
        [HttpGet("{token}")]
        public async Task<ActionResult<User>> GetSession(string token)
        {
          if (_context.Session == null)
          {
              return NotFound();
          }
          //  Получение сессии из базы данных по токену
          var session = _context.Session.FirstOrDefault(s => s.SessionToken == token);
          if (session == null)
          {
              return NotFound("Token doesn't exist");
          }
          // Получение пользователя из базы данных по соотнесению Id и UserId сессии
          var user = _context.User!.FirstOrDefault(u => u.Id == session.UserId);
          return user!;
        }
    }
}
