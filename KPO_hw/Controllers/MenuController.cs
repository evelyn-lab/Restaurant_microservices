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
using KPO_hw.Context;
using KPO_hw.Models;

namespace KPO_hw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly DataContext _context;

        public MenuController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Menu
        [HttpGet]
        public IActionResult GetMenu()
        {
          if (_context.Dish == null)
          {
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
          return Ok(text);
        }

        private bool DishExists(int id)
        {
            return (_context.Dish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
