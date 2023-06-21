namespace Order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 *  Aтрибут указывает, что класс Session соответствует таблице
 *  "session" в базе данных.
 */
[Table("session")]
public class Session
{
    // Атрибут указывает, что свойство Id - первичный ключ для таблицы
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("session_token")]
    public string? SessionToken { get; set; }
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }
}