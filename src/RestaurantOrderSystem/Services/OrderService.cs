using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class OrderService : IOrderService
{
    private static int _orderCounter = 1;

    public Order CreateOrder()
    {
        return new Order { OrderId = _orderCounter++ };
    }

    public void AddItemToOrder(Order order, MenuItem item)
    {
        order.Items.Add(item);
        order.Subtotal = CalculateSubtotal(order);
    }

    public void RemoveItemFromOrder(Order order, MenuItem item)
    {
        order.Items.Remove(item);
        order.Subtotal = CalculateSubtotal(order);
    }

    public decimal CalculateSubtotal(Order order)
    {
        return order.Items.Sum(item => item.Price);
    }
}
