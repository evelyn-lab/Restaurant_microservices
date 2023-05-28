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
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Order == null)
          {
              return NotFound("Order doesn't exist.");
          }
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order doesn't exist.");
            }

            return order;
        }
        
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(UserOrder userOrder)
        {
          if (_context.Dish == null)
          {
              return Problem("Entity set 'DataContext.Dish'  is null.");
          }

          if (_context.User != null)
          {
              var user = _context.User.FirstOrDefault(u => u.UserName == userOrder.UserName);
              if (user == null)
              {
                  return NotFound("Username doesn't exist.");
              }
              var dishList = new List<KeyValuePair<Dish, int>>();
              foreach (var item in userOrder.DishList)
              {
                  var dish = _context.Dish.FirstOrDefault(d => d.Name == item.Name);
                  if (dish == null)
                  {
                      return NotFound("Dish doesn't exist in the menu.");
                  }

                  if (dish.Quantity < item.Quantity)
                  {
                      return Problem("Requested quantity of dish cannot be provided.");
                  }
                  dishList.Add(new KeyValuePair<Dish, int>(dish, item.Quantity));
              }
              Order order = new Order();
              order.Status = "in wait";
              order.UserId = user.Id;
              order.CreatedAt = DateTime.Now;
              order.UpdatedAt = DateTime.Now;
              order.SpecialRequests = userOrder.SpecialRequests;
              _context.Order.Add(order);
              await _context.SaveChangesAsync();
              order.ChangeStatus(order, _context);
              foreach (var item in dishList)
              {
                  var orderDish = new OrderDish();
                  orderDish.OrderId = order.Id;
                  orderDish.DishId = item.Key.Id;
                  orderDish.Quantity = item.Value;
                  orderDish.Price = item.Key.Price;
                  _context.OrderDish.Add(orderDish);
                  await _context.SaveChangesAsync();
                  item.Key.Quantity -= item.Value;
                  if (item.Key.Quantity == 0)
                  {
                      item.Key.IsAvailable = false;
                  }
                  await _context.SaveChangesAsync();
              }
              return Ok("Order created. Order Id is: " + order.Id.ToString());
          }
          else
          {
              return Problem("Entity set 'DataContext.User'  is null.");
          }
        }

        private bool OrderExists(int id)
        {
            return (_context.Dish?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
