namespace RestaurantOrderSystem.Models;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsComboEligible { get; set; } = true;
    public TimeOfDay BestTimeToServe { get; set; } = TimeOfDay.Lunch;

    // Dynamic price based on time of day
    public decimal GetCurrentPrice(TimeOfDay timeOfDay)
    {
        var multiplier = timeOfDay switch
        {
            TimeOfDay.Morning => 0.9m,
            TimeOfDay.Lunch => 1.0m,
            TimeOfDay.Afternoon => 0.95m,
            TimeOfDay.Dinner => 1.15m,
            TimeOfDay.LateNight => 1.1m,
            _ => 1.0m
        };

        // Special breakfast discount for breakfast items
        if (Category == "Breakfast" && timeOfDay == TimeOfDay.Morning)
        {
            multiplier *= 0.8m; // Additional 20% off
        }

        return BasePrice * multiplier;
    }

    public MenuItem(int id, string name, decimal basePrice, string category, string description = "")
    {
        Id = id;
        Name = name;
        BasePrice = basePrice;
        Category = category;
        Description = description;
    }
}