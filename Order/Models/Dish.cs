namespace Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 *  Aтрибут указывает, что класс Dish соответствует таблице
 *  "dish" в базе данных.
 */
[Table("dish")]
public class Dish
{
    // Атрибут указывает, что свойство Id - первичный ключ для таблицы
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("is_available")]
    public bool IsAvailable { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}