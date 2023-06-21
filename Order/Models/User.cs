namespace Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 *  Aтрибут указывает, что класс User соответствует таблице
 *  "user" в базе данных.
 */
[Table("user")]
public class User
{
    // Атрибут указывает, что свойство Id - первичный ключ для таблицы
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string? UserName { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("password_hash")]
    public string? Password { get; set; }
    [Column("role")]
    public string? Role { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}