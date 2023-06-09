using Order.Models;

namespace Order.Context;
using Microsoft.EntityFrameworkCore;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User>? User { get; set; }
    public DbSet<Session>? Session { get; set; }
    public DbSet<Dish>? Dish { get; set; }
    
    public DbSet<Models.Order>? Order { get; set; }
    
    public DbSet<OrderDish>? OrderDish { get; set; }
}