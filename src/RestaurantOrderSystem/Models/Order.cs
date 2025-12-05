namespace RestaurantOrderSystem.Models;

public class Order
{
    public int OrderId { get; set; }
    public List<MenuItem> Items { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    // New compatibility field used by AddComboToOrder in OrderService
    public decimal ComboDiscounts { get; set; }

    public Order()
    {
        Items = new List<MenuItem>();
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
}