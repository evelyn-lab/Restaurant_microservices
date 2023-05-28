namespace KPO_hw.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("user")]
public class User
{
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