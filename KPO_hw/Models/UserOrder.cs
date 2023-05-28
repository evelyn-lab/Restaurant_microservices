namespace KPO_hw.Models;

public class OrderItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}


public class UserOrder
{
    public string UserName { get; set; }
    public List<OrderItem> DishList {get; set;}
    public string? SpecialRequests { get; set; }
}