namespace RestaurantOrderSystem.Models;

public class LoyaltyCustomer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; }
    public string Tier
    {
        get
        {
            if (Points >= 1000) return "Platinum";
            if (Points >= 500) return "Gold";
            if (Points >= 100) return "Silver";
            return "Bronze";
        }
    }
}