using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Context;
using Order.Models;

namespace Order.Controllers
{
    // Атрибут указывает маршрут, по которому будет доступен контроллер.
    [Route("api/[controller]")]
    // Атрибут указывает, что данный контроллер является контроллером API
    [ApiController]
    
    // Класс контроллера заказа пользователя
    public class OrderController : ControllerBase
    {
        // Контекст данных для взаимодействия с базой данных
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Order.Models.Order>> GetOrder(int id)
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
        // Метод обработки HTTP POST-запроса для создания нового заказа на основе данных,
        // предоставленных объектом UserOrder
        [HttpPost]
        public async Task<ActionResult<Models.Order>> PostOrder(UserOrder userOrder)
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
              // Проверка и обработка списка блюд userOrder.DishList
              foreach (var item in userOrder.DishList)
              {
                  // Получение блюда из базы данных по названию
                  var dish = _context.Dish.FirstOrDefault(d => d.Name == item.Name);
                  if (dish == null)
                  {
                      return NotFound("Dish doesn't exist in the menu.");
                  }

                  if (dish.Quantity < item.Quantity)
                  {
                      return Problem("Requested quantity of dish cannot be provided.");
                  }
                  // Если все данные валидные, добавляем в dishList пару {блюдо:количество}
                  dishList.Add(new KeyValuePair<Dish, int>(dish, item.Quantity));
              }
              Order.Models.Order order = new Order.Models.Order
              {
                  Status = "in wait",
                  UserId = user.Id,
                  CreatedAt = DateTime.Now,
                  UpdatedAt = DateTime.Now,
                  SpecialRequests = userOrder.SpecialRequests
              };
              _context.Order?.Add(order);
              await _context.SaveChangesAsync();
              // Запускаем обработку статуса заказа
              await order.ChangeStatus(order, _context);
              foreach (var item in dishList)
              {
                  var orderDish = new OrderDish
                  {
                      OrderId = order.Id,
                      DishId = item.Key.Id,
                      Quantity = item.Value,
                      Price = item.Key.Price
                  };
                  _context.OrderDish?.Add(orderDish);
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
    }
}
