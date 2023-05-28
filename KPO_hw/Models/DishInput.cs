namespace KPO_hw.Models;

/*
 * Класс полей блюда для метода Post в DishController
 * Name - название блюда
 * Description - описание блюда
 * Price -  цена блюда
 * Quantity - количество блюда 
 */
public class DishInput
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}