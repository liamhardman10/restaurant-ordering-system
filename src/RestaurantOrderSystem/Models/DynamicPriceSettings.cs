namespace RestaurantOrderSystem.Models;

public enum TimeOfDay
{
    Morning,    // 6 AM - 11 AM
    Lunch,      // 11 AM - 2 PM
    Afternoon,  // 2 PM - 5 PM
    Dinner,     // 5 PM - 9 PM
    LateNight   // 9 PM - 6 AM
}

public class DynamicPriceSettings
{
    public TimeOfDay CurrentTimeOfDay { get; set; }
    public decimal MorningMultiplier { get; set; } = 0.9m;   // 10% discount mornings
    public decimal LunchMultiplier { get; set; } = 1.0m;     // Standard price
    public decimal DinnerMultiplier { get; set; } = 1.15m;   // 15% premium dinner
    public decimal LateNightMultiplier { get; set; } = 1.1m; // 10% premium late night

    public decimal GetPriceMultiplier()
    {
        return CurrentTimeOfDay switch
        {
            TimeOfDay.Morning => MorningMultiplier,
            TimeOfDay.Lunch => LunchMultiplier,
            TimeOfDay.Afternoon => LunchMultiplier,
            TimeOfDay.Dinner => DinnerMultiplier,
            TimeOfDay.LateNight => LateNightMultiplier,
            _ => 1.0m
        };
    }
}
