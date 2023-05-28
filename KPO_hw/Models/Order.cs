using KPO_hw.Context;

namespace KPO_hw.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("order")]
public class Order
{
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
    
    // Обновление статуса заказа
    public async Task ChangeStatus(Order order, DataContext _context)
    {
        await Task.Delay(5000);
        order.Status = "In progress"; 
        await Task.Delay(5000);
        order.Status = "Completed"; 
        await _context.SaveChangesAsync();;
    }
}
