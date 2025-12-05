namespace RestaurantOrderSystem.Models;

public enum LoyaltyTier
{
    Bronze,     // 0-99 points
    Silver,     // 100-499 points
    Gold,       // 500-999 points
    Platinum    // 1000+ points
}

public class LoyaltyCustomer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; }
    public LoyaltyTier Tier => CalculateTier();

    private LoyaltyTier CalculateTier()
    {
        return Points switch
        {
            < 100 => LoyaltyTier.Bronze,
            < 500 => LoyaltyTier.Silver,
            < 1000 => LoyaltyTier.Gold,
            _ => LoyaltyTier.Platinum
        };
    }

    public decimal GetTierDiscount()
    {
        return Tier switch
        {
            LoyaltyTier.Bronze => 0.0m,
            LoyaltyTier.Silver => 0.05m,   // 5% discount
            LoyaltyTier.Gold => 0.10m,     // 10% discount
            LoyaltyTier.Platinum => 0.15m, // 15% discount
            _ => 0.0m
        };
    }
}
