using KPO_hw.Models;

namespace KPO_hw.Context;
using Microsoft.EntityFrameworkCore;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    public DbSet<User>? User { get; set; }
    public DbSet<Session>? Session { get; set; }
}