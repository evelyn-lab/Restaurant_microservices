namespace Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 *  Aтрибут указывает, что класс OrderDish соответствует таблице
 *  "order_dish" в базе данных.
 */
[Table("order_dish")]
public class OrderDish
{
    // Атрибут указывает, что свойство Id - первичный ключ для таблицы
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