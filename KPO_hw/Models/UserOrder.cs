namespace KPO_hw.Models;

public struct OrderItem
{
    private string Name;
    private int Quantity;
    OrderItem(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}

public class OrderForm
{
    public int UserId { get; set; }
    public List<OrderItem> DishList {get; set;}
    public string? SpecialRequests { get; set; }
}

public class UserOrder
{
    public int UserId { get; set; }
    public List<OrderItem> DishList {get; set;}
    public string? Status { get; set; }
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}