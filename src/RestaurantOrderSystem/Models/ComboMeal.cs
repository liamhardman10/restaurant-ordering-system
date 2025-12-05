using System.Collections.Generic;

namespace RestaurantOrderSystem.Models;

public class ComboMeal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<MenuItem> Items { get; set; } = new();
    public decimal ComboPrice { get; set; }
    // Computed savings (sum of base prices - combo price)
    public decimal Savings => Items.Sum(i => i.Price) - ComboPrice;

    public bool IsValid() => Items != null && Items.Count > 0 && ComboPrice >= 0;
}
