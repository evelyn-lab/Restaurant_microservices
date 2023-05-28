using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KPO_hw.Context;
using KPO_hw.Models;
using Microsoft.AspNetCore.Authorization;

namespace KPO_hw.Controllers
{
    // Атрибут указывает маршрут, по которому будет доступен контроллер.
    [Route("api/[controller]")]
    // Атрибут указывает, что данный контроллер является контроллером API
    [ApiController]
    
    // Класс контроллера блюд
    public class DishController : ControllerBase
    {
        // Контекст данных для взаимодействия с базой данных
        private readonly DataContext _context;

        public DishController(DataContext context)
        {
            _context = context;
        }
        
        //  Атрибут, указывающий, что метод обрабатывает GET-запросы
        [HttpGet]
        // Атрибут, ограничивающий доступ к методу только для пользователей с ролью "manager"
        [Authorize(Roles = "manager")]
        // Метод, возвращающий список всех блюд
        public async Task<ActionResult<IEnumerable<Dish>>> GetDish()
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            if (_context.Dish == null) 
            {
              return NotFound("Dish database is empty"); 
            }
            return await _context.Dish.ToListAsync();
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]
        // Метод, возвращающий блюдо по указанному идентификатору 
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            if (_context.Dish == null)
            {
                return NotFound("Dish database is empty");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound("No dish with such id");
            }
            return dish;
        }
        
        // Атрибут, указывающий, что метод обрабатывает PUT-запросы с параметром идентификатора
        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        // Метод, обновляющий информацию о блюде с указанным идентификатором
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            if (id != dish.Id)
            {
                return BadRequest();
            }
            _context.Entry(dish).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        
        // Атрибут, указывающий, что метод обрабатывает POST-запросы
        [HttpPost]
        [Authorize(Roles = "manager")]
        // Метод, создающий новое блюдо на основе переданных данных
        public async Task<ActionResult<Dish>> PostDish(DishInput dishInput)
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            Dish dish = new Dish
            {
                Name = dishInput.Name,
                Description = dishInput.Description,
                Price = dishInput.Price,
                Quantity = dishInput.Quantity,
                IsAvailable = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Dish?.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }
        // Атрибут, указывающий, что метод обрабатывает DELETE-запросы с параметром идентификатора
        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        // Метод, удаляющий блюдо с указанным идентификатором
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            if (_context.Dish == null)
            {
                return NotFound();
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        // Вспомогательный метод, проверяющий, существует ли блюдо с указанным идентификатором в базе данных
        private bool DishExists(int id)
        {
            return (_context.Dish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
