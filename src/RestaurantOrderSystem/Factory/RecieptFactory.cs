using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Factories;

public class ReceiptFactory
{
    public Receipt CreateReceipt(Order order)
    {
        return new Receipt
        {
            OrderId = order.OrderId,
            Items = new List<MenuItem>(order.Items),
            Subtotal = order.Subtotal,
            Tax = order.Tax,
            DiscountAmount = order.DiscountAmount,
            Total = order.Total,
            Timestamp = DateTime.Now
        };
    }
}
