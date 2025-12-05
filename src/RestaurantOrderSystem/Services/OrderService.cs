using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class OrderService : IOrderService
{
    private static int _orderCounter = 1;
    private readonly IDynamicPricingService _dynamicPricingService;

    public OrderService()
    {
        _dynamicPricingService = new DynamicPricingService();
    }

    public OrderService(IDynamicPricingService dynamicPricingService)
    {
        _dynamicPricingService = dynamicPricingService;
    }

    public Order CreateOrder()
    {
        return new Order { OrderId = _orderCounter++ };
    }

    public void AddItemToOrder(Order order, MenuItem item)
    {
        // Apply dynamic pricing before adding
        var currentPrice = _dynamicPricingService.ApplyTimeBasedPricing(
            item.BasePrice,
            item.Category
        );

        // Create a priced item (could be a separate class)
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
        var timeOfDay = _dynamicPricingService.GetCurrentTimeOfDay();
        return order.Items.Sum(item => item.GetCurrentPrice(timeOfDay));
    }

    // New method for combo meals
    public void AddComboToOrder(Order order, ComboMeal combo)
    {
        if (!combo.IsValid())
            throw new ArgumentException("Invalid combo meal");

        // Add all items from combo
        foreach (var item in combo.Items)
        {
            order.Items.Add(item);
        }

        // Apply combo discount
        order.ComboDiscounts += combo.Savings;
        order.Subtotal = CalculateSubtotal(order);
    }
}