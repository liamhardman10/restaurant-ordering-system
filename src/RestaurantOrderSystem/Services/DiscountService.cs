using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class DiscountService : IDiscountService
{
    public Discount? GetDiscount(string code, List<Discount> availableDiscounts)
    {
        return availableDiscounts.FirstOrDefault(d => d.Code == code);
    }

    public decimal CalculateDiscount(decimal amount, Discount? discount)
    {
        if (discount == null) return 0;

        return discount.Type == DiscountType.Percentage
            ? amount * (discount.Value / 100)
            : discount.Value;
    }
}
