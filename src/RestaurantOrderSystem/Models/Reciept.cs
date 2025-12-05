namespace RestaurantOrderSystem.Models;

public class Receipt
{
    public int OrderId { get; set; }
    public List<MenuItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public DateTime Timestamp { get; set; }

    public string GenerateReceiptText()
    {
        return $$"""
            Order #{{OrderId}}
            Date: {{Timestamp:yyyy-MM-dd HH:mm:ss}}
            Items: {{Items.Count}}
            ----------------------------
            {{GetItemsText()}}
            ----------------------------
            Subtotal: ${{Subtotal:F2}}
            Tax: ${{Tax:F2}}
            Discount: ${{DiscountAmount:F2}}
            Total: ${{Total:F2}}
            """;
    }

    private string GetItemsText()
    {
        return string.Join("\n", Items.Select(item =>
            $"{item.Name,-20} ${item.Price:F2}"));
    }
}