namespace KPO_hw.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("order_dish")]
public class OrderDish
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_id")]
    public int OrderId { get; set; }
    [Column("dish_id")]
    public int DishId { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
}