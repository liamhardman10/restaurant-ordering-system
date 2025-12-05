using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public interface IPricingService
{
    decimal CalculateTax(decimal amount);
    decimal ApplyDiscount(decimal amount, Discount discount);
    decimal CalculateTotal(Order order);
}