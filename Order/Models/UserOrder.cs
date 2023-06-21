namespace Order.Models;

/*
 * Вспомогательный класс элемента заказа
 * Name - название блюда
 * Quantity - количество блюд данного типа
 */
public class OrderItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}

/*
 * Класс полей заказа пользователя для метода Post в OrderController
 * UserName - имя пользователя
 * DishList - список элементов заказа
 * SpecialRequests -  специальные запросы пользователя, связанные с заказом
 */
public class UserOrder
{
    public string UserName { get; set; }
    public List<OrderItem> DishList {get; set;}
    public string? SpecialRequests { get; set; }
}