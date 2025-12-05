using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public interface IOrderService
{
    Order CreateOrder();
    void AddItemToOrder(Order order, MenuItem item);
    void RemoveItemFromOrder(Order order, MenuItem item);
    decimal CalculateSubtotal(Order order);
}