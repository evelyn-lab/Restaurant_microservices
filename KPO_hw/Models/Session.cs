namespace KPO_hw.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("session")]
public class Session
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("session_token")]
    public string SessionToken { get; set; }
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }
}