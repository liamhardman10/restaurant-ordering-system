namespace RestaurantOrderSystem.Services;

public enum TimeOfDaySlot { Morning, Afternoon, Evening, Night }

public interface IDynamicPricingService
{
    TimeOfDaySlot GetCurrentTimeOfDay();
    // Applies time-based multiplier or returns adjusted price
    decimal ApplyTimeBasedPricing(decimal basePrice, string category);
}