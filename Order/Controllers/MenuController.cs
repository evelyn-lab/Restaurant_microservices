using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Order.Context;
using Order.Models;

namespace Order.Controllers
{
    // Атрибут указывает маршрут, по которому будет доступен контроллер.
    [Route("api/[controller]")]
    // Атрибут указывает, что данный контроллер является контроллером API
    [ApiController]
    
    // Класс контроллера предоставления меню
    public class MenuController : ControllerBase
    {
        // Контекст данных для взаимодействия с базой данных
        private readonly DataContext _context;

        public MenuController(DataContext context)
        {
            _context = context;
        }
        
        // Метод обработки HTTP GET-запроса для получения информации о меню
        [HttpGet]
        public IActionResult GetMenu()
        {
          if (_context.Dish == null)
          {
              // Возвращается ошибка 404 и сообщение "Menu is empty"
              return NotFound("Menu is empty");
          }
          string text = "";
          foreach (var dish in _context.Dish)
          {
              if (dish.IsAvailable)
              {
                  text += "Name: " + dish.Name + "\n";
                  text += "Description: " + dish.Description + "\n";
                  text += "Price: " + dish.Price + "\n";
                  text += "Quantity: " + dish.Quantity + "\n\n";
              }
          }
          // Возвращается ответ с успешным кодом 200 и текстом, содержащим информацию о меню
          return Ok(text);
        }
    }
}
