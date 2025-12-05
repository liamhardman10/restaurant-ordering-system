using System;
using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public interface IDynamicPricingService
{
    TimeOfDaySlot GetCurrentTimeOfDay();
    decimal ApplyTimeBasedPricing(decimal basePrice, string category);
    decimal ApplyLoyaltyDiscount(decimal price, LoyaltyTier tier);
    decimal CalculateComboDiscount(List<MenuItem> items);
}

public class DynamicPricingService : IDynamicPricingService
{
    public TimeOfDaySlot GetCurrentTimeOfDay()
    {
        var hour = DateTime.Now.Hour;
        if (hour >= 6 && hour < 11) return TimeOfDaySlot.Morning;
        if (hour >= 11 && hour < 16) return TimeOfDaySlot.Afternoon;
        if (hour >= 16 && hour < 22) return TimeOfDaySlot.Evening;
        return TimeOfDaySlot.Night;
    }

    public decimal ApplyTimeBasedPricing(decimal basePrice, string category)
    {
        // Simple example: evening surcharge 10%, night discount 10%
        var slot = GetCurrentTimeOfDay();
        return slot switch
        {
            TimeOfDaySlot.Evening => Math.Round(basePrice * 1.10m, 2),
            TimeOfDaySlot.Night => Math.Round(basePrice * 0.90m, 2),
            _ => basePrice
        };
    }

    public decimal ApplyLoyaltyDiscount(decimal price, LoyaltyTier tier)
    {
        var discount = tier switch
        {
            LoyaltyTier.Bronze => 0.0m,
            LoyaltyTier.Silver => 0.05m,
            LoyaltyTier.Gold => 0.10m,
            LoyaltyTier.Platinum => 0.15m,
            _ => 0.0m
        };

        return price * (1 - discount);
    }

    public decimal CalculateComboDiscount(List<MenuItem> items)
    {
        if (items.Count < 2) return 0;

        var totalBasePrice = items.Sum(item => item.BasePrice);
        var comboDiscount = items.Count switch
        {
            2 => 0.10m, // 10% off for 2 items
            3 => 0.15m, // 15% off for 3 items
            >= 4 => 0.20m, // 20% off for 4+ items
            _ => 0.0m
        };

        return totalBasePrice * comboDiscount;
    }
}