namespace RestaurantOrderSystem.Models;

public enum DiscountType
{
    Percentage,
    FixedAmount
}

public class Discount
{
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
}