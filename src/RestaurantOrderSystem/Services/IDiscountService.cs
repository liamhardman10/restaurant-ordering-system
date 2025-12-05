using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public interface IDiscountService
{
    Discount? GetDiscount(string code, List<Discount> availableDiscounts);
    decimal CalculateDiscount(decimal amount, Discount? discount);
}
