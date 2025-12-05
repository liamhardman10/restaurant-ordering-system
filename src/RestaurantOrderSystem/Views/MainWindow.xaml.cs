using RestaurantOrderSystem.Factories;
using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;
using System.Windows;
using System.Windows.Controls;

public partial class MainWindow : Window
{
    // ... existing fields ...
    private readonly IDynamicPricingService _dynamicPricingService;
    private LoyaltyCustomer? _currentCustomer;
    private List<ComboMeal> _comboMeals = [];

    public MainWindow()
    {
        InitializeComponent();

        // Initialize services
        _orderService = new OrderService();
        _pricingService = new PricingService();
        _discountService = new DiscountService();
        _receiptFactory = new ReceiptFactory();
        _dynamicPricingService = new DynamicPricingService();

        private Order _currentOrder = null!;
        private List<ModelMenuItem> _menuItems = new List<ModelMenuItem>();
        private List<Discount> _availableDiscounts = new List<Discount>();

        private LoyaltyCustomer? _currentCustomer;
        private List<ComboMeal> _comboMeals = new List<ComboMeal>();

        public MainWindow()
        {
            InitializeComponent();

            // Initialize services
            _orderService = new OrderService();
            _pricingService = new PricingService();
            _discountService = new DiscountService();
            _receiptFactory = new ReceiptFactory();
            _dynamicPricingService = new DynamicPricingService();

            InitializeMenu();
            InitializeDiscounts();
            InitializeComboMeals();
            StartNewOrder();
            UpdateTimeBasedPricingDisplay();
        }

         // Update pricing indicators
        foreach (var item in _menuItems)
        {
            var currentPrice = item.GetCurrentPrice(timeOfDay);
            var priceDiff = currentPrice - item.BasePrice;

            MenuItemsControl.ItemsSource = _menuItems;
        }

        private void InitializeDiscounts()
        {
            _availableDiscounts = new List<Discount>
            {
                new Discount { Code = "SAVE10", Type = DiscountType.Percentage, Value = 10, Description = "10% off your order" },
                new Discount { Code = "SAVE5", Type = DiscountType.Percentage, Value = 5, Description = "5% off your order" },
                new Discount { Code = "FIXED3", Type = DiscountType.FixedAmount, Value = 3, Description = "$3 off your order" }
            };
        }

        private void InitializeComboMeals()
        {
            // Ensure _menuItems populated
            if (!_menuItems.Any())
                InitializeMenu();

            var burgerItem = _menuItems.FirstOrDefault(m => m.Name.Contains("Cheeseburger", StringComparison.OrdinalIgnoreCase));
            var friesItem = new ModelMenuItem(11, "French Fries", 3.99m, "Side", "Crispy golden fries");
            var drinkItem = _menuItems.FirstOrDefault(m => m.Name.Contains("Iced Tea", StringComparison.OrdinalIgnoreCase));

            // Defensive: ensure required items exist
            if (burgerItem == null || drinkItem == null)
            {
                // If items missing, create reasonable fallbacks or skip combo population
                return;
            }

            _comboMeals = new List<ComboMeal>
            {
                new ComboMeal
                {
                    Id = 1,
                    Name = "Burger Combo",
                    Description = "Cheeseburger + Fries + Drink",
                    Items = new List<ModelMenuItem> { burgerItem, friesItem, drinkItem },
                    ComboPrice = 12.99m
                },
                new ComboMeal
                {
                    Id = 2,
                    Name = "Lunch Special",
                    Description = "Soup + Salad + Drink",
                    Items = new List<ModelMenuItem>
                    {
                        _menuItems.First(m => m.Name.Contains("Soup", StringComparison.OrdinalIgnoreCase)),
                        _menuItems.First(m => m.Name.Contains("Salad", StringComparison.OrdinalIgnoreCase)),
                        drinkItem
                    },
                    ComboPrice = 9.99m
                }
            };

            ComboMealsControl.ItemsSource = _comboMeals;
        }

        private void UpdateTimeBasedPricingDisplay()
        {
            // IDynamicPricingService and MenuItem APIs vary by implementation.
            // This code assumes: GetCurrentTimeOfDay() and MenuItem.GetCurrentPrice(timeOfDay) exist.
            var timeOfDay = _dynamicPricingService.GetCurrentTimeOfDay();
            TimeOfDayText.Text = $"Current: {timeOfDay}";

            foreach (var item in _menuItems)
            {
                decimal currentPrice;
                // Prefer GetCurrentPrice if provided, otherwise fall back to Price
                try
                {
                    // If MenuItem exposes GetCurrentPrice, call it; otherwise use Price property
                    var method = item.GetType().GetMethod("GetCurrentPrice", new[] { timeOfDay.GetType() });
                    if (method != null)
                    {
                        currentPrice = (decimal)method.Invoke(item, new object[] { timeOfDay })!;
                    }
                    else
                    {
                        currentPrice = item.Price;
                    }
                }
                catch
                {
                    currentPrice = item.Price;
                }

                var basePrice = item.Price;
                var priceDiff = currentPrice - basePrice;

    // New event handler for combo meals
private void AddComboButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int comboId)
        {
            var combo = _comboMeals.FirstOrDefault(c => c.Id == comboId);
            if (combo != null)
            {
                try
                {
                    _orderService.AddComboToOrder(_currentOrder, combo);
                    UpdateOrderDisplay();
                    StatusText.Text = $"Added '{combo.Name}' combo (Save ${combo.Savings:F2})";
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // Loyalty login method
    private void LoginCustomerButton_Click(object sender, RoutedEventArgs e)
    {
        // Simple mock customer login
        _currentCustomer = new LoyaltyCustomer
        {
            CustomerId = 1,
            Name = "John Doe",
            Points = 650 // Gold tier
        };

        CustomerNameText.Text = _currentCustomer.Name;
        LoyaltyTierText.Text = $"Tier: {_currentCustomer.Tier}";
        PointsText.Text = $"Points: {_currentCustomer.Points}";

        StatusText.Text = $"Welcome back, {_currentCustomer.Name}! ({_currentCustomer.Tier} member)";
    }

    // Updated checkout to award points
    private void CheckoutButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentOrder.Items.Count == 0)
        {
            MessageBox.Show(
                "Your order is empty. Please add items from the menu before checking out.",
                "Empty Order",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
            return;
        }

        // Award loyalty points if customer is logged in
        if (_currentCustomer != null)
        {
            int pointsEarned = (int)(_currentOrder.Total * 10); // 10 points per dollar
            _currentCustomer.Points += pointsEarned;
            PointsText.Text = $"Points: {_currentCustomer.Points}";

            // Check if tier changed
            var newTier = _currentCustomer.Tier;
            LoyaltyTierText.Text = $"Tier: {newTier}";
        }

        // Create receipt
        var receipt = _receiptFactory.CreateReceipt(_currentOrder);

        // Add loyalty info to receipt
        if (_currentCustomer != null)
        {
            receipt.CustomerName = _currentCustomer.Name;
            receipt.LoyaltyTier = _currentCustomer.Tier;
            receipt.PointsEarned = (int)(_currentOrder.Total * 10);
        }

        // Show receipt
        MessageBox.Show(
            receipt.GenerateReceiptText(),
            $"Order #{receipt.OrderId} - Receipt",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );

        // Start new order
        StartNewOrder();
        DiscountTextBox.Clear();
        StatusText.Text = "Order completed. New order started.";
    }

    // ... rest of existing methods ...
}