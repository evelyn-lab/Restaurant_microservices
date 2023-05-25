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
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly DataContext _context;

        public DishController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Roles = "manager")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
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

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Dish>> PostDish(DishInput dishInput)
        {
            if (!User.IsInRole("manager"))
            {
                return Problem("Role has to be manager");
            }
            Dish dish = new Dish();
            dish.Name = dishInput.Name;
            dish.Description = dishInput.Description;
            dish.Price = dishInput.Price;
            dish.Quantity = dishInput.Quantity;
            dish.IsAvailable = true;
            dish.CreatedAt = DateTime.Now;
            dish.UpdatedAt = DateTime.Now;
            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
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

        private bool DishExists(int id)
        {
            return (_context.Dish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
