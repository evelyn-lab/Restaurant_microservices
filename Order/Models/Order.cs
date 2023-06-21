using Order.Context;

namespace Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

/*
 *  Aтрибут указывает, что класс Order соответствует таблице
 *  "order" в базе данных.
 */
[Table("order")]
public class Order
{
    // Атрибут указывает, что свойство Id - первичный ключ для таблицы
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("status")]
    public string? Status { get; set; }
    [Column("special_requests")]
    public string? SpecialRequests { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    // Функция обновления статуса заказа
    public async Task ChangeStatus(Order order, DataContext _context)
    {
        await Task.Delay(5000);
        order.Status = "In progress"; 
        // асинхронно сохраняем изменения в базе данных
        await _context.SaveChangesAsync();;
        await Task.Delay(5000);
        order.Status = "Completed";
        await _context.SaveChangesAsync();
    }
}
