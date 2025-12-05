using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class PricingService : IPricingService
{
    private const decimal TaxRate = 0.08m;

    public decimal CalculateTax(decimal amount)
    {
        return amount * TaxRate;
    }

    public decimal ApplyDiscount(decimal amount, Discount discount)
    {
        if (discount == null) return amount;

        return discount.Type == DiscountType.Percentage
            ? amount * (1 - discount.Value / 100)
            : amount - discount.Value;
    }

    public decimal CalculateTotal(Order order)
    {
        return order.Subtotal + order.Tax - order.DiscountAmount;
    }
}
