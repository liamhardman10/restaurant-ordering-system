using System.Collections.Generic;
using System.Linq;
using RestaurantOrderSystem.Factories;
using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;
using System.Windows;
using System.Windows.Controls;

// Alias the model type to avoid ambiguity with System.Windows.Controls.MenuItem
using ModelMenuItem = RestaurantOrderSystem.Models.MenuItem;

namespace RestaurantOrderSystem.View
{
    public partial class MainWindow : Window
    {
        private readonly IOrderService _orderService;
        private readonly IPricingService _pricingService;
        private readonly IDiscountService _discountService;
        private readonly ReceiptFactory _receiptFactory;

        private Order _currentOrder = null!;
        private List<ModelMenuItem> _menuItems = new List<ModelMenuItem>();
        private List<Discount> _availableDiscounts = new List<Discount>();

        public MainWindow()
        {
            InitializeComponent();

            // Initialize services
            _orderService = new OrderService();
            _pricingService = new PricingService();
            _discountService = new DiscountService();
            _receiptFactory = new ReceiptFactory();

            InitializeMenu();
            InitializeDiscounts();
            StartNewOrder();
        }

        private void InitializeMenu()
        {
            _menuItems = new List<ModelMenuItem>
            {
                new ModelMenuItem(1, "Classic Cheeseburger", 8.99m, "Main", "Beef patty with cheese, lettuce, and tomato"),
                new ModelMenuItem(2, "Caesar Salad", 6.99m, "Salad", "Fresh romaine with Caesar dressing and croutons"),
                new ModelMenuItem(3, "Spaghetti Bolognese", 12.99m, "Main", "Pasta with homemade meat sauce"),
                new ModelMenuItem(4, "Iced Tea", 2.49m, "Drink", "Freshly brewed iced tea"),
                new ModelMenuItem(5, "Chocolate Lava Cake", 5.99m, "Dessert", "Warm chocolate cake with molten center"),
                new ModelMenuItem(6, "Fish & Chips", 10.99m, "Main", "Beer-battered cod with fries"),
                new ModelMenuItem(7, "French Onion Soup", 4.99m, "Appetizer", "With melted cheese and crouton"),
                new ModelMenuItem(8, "Coffee", 1.99m, "Drink", "Freshly brewed coffee"),
                new ModelMenuItem(9, "Chicken Wings", 8.49m, "Appetizer", "10 pieces with your choice of sauce"),
                new ModelMenuItem(10, "Margherita Pizza", 11.99m, "Main", "Classic tomato, mozzarella, and basil")
            };

            // Bind to ItemsControl defined in XAML (ensure XAML x:Name matches 'MenuItemsControl')
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

        private void StartNewOrder()
        {
            _currentOrder = _orderService.CreateOrder();
            UpdateOrderDisplay();
            StatusText.Text = $"New order #{_currentOrder.OrderId} started";
            OrderIdText.Text = $"Order #{_currentOrder.OrderId}";
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int itemId)
            {
                var selectedItem = _menuItems.FirstOrDefault(item => item.Id == itemId);
                if (selectedItem != null)
                {
                    _orderService.AddItemToOrder(_currentOrder, selectedItem);
                    UpdateOrderDisplay();
                    StatusText.Text = $"Added '{selectedItem.Name}' to order";
                }
            }
        }

        private void RemoveOrderItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int itemId)
            {
                var item = _currentOrder.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    _orderService.RemoveItemFromOrder(_currentOrder, item);
                    UpdateOrderDisplay();
                    StatusText.Text = $"Removed '{item.Name}' from order";
                }
            }
        }

        // Handler bound from XAML: ItemsControl item template uses Click="RemoveItemButton_Click"
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                // Tag could be string/int depending on binding; attempt to parse to int
                if (button.Tag is int id)
                {
                    var item = _currentOrder.Items.FirstOrDefault(i => i.Id == id);
                    if (item != null)
                    {
                        _orderService.RemoveItemFromOrder(_currentOrder, item);
                        UpdateOrderDisplay();
                        StatusText.Text = $"Removed '{item.Name}' from order";
                    }
                }
                else if (int.TryParse(button.Tag.ToString(), out var parsedId))
                {
                    var item = _currentOrder.Items.FirstOrDefault(i => i.Id == parsedId);
                    if (item != null)
                    {
                        _orderService.RemoveItemFromOrder(_currentOrder, item);
                        UpdateOrderDisplay();
                        StatusText.Text = $"Removed '{item.Name}' from order";
                    }
                }
            }
        }

        private void ApplyDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            string code = DiscountTextBox.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                StatusText.Text = "Please enter a discount code";
                return;
            }

            var discount = _discountService.GetDiscount(code, _availableDiscounts);
            if (discount != null)
            {
                _currentOrder.DiscountAmount = _discountService.CalculateDiscount(_currentOrder.Subtotal, discount);
                UpdateOrderDisplay();
                StatusText.Text = $"Applied discount: {discount.Description}";
            }
            else
            {
                StatusText.Text = $"Invalid discount code '{code}'. Try: SAVE10, SAVE5, FIXED3";
            }
        }

        private void UpdateOrderDisplay()
        {
            // Update order items
            OrderItemsControl.ItemsSource = null;
            OrderItemsControl.ItemsSource = _currentOrder.Items;

            // Show/hide empty order message (use XAML-generated field)
            if (EmptyOrderText != null)
            {
                EmptyOrderText.Visibility = _currentOrder.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            // Calculate totals
            _currentOrder.Subtotal = _orderService.CalculateSubtotal(_currentOrder);
            _currentOrder.Tax = _pricingService.CalculateTax(_currentOrder.Subtotal);
            _currentOrder.Total = _pricingService.CalculateTotal(_currentOrder);

            // Update UI
            SubtotalText.Text = $"${_currentOrder.Subtotal:F2}";
            TaxText.Text = $"${_currentOrder.Tax:F2}";
            DiscountText.Text = $"${_currentOrder.DiscountAmount:F2}";
            TotalText.Text = $"${_currentOrder.Total:F2}";
            ItemCountText.Text = _currentOrder.Items.Count.ToString();

            // Update button states
            CheckoutButton.IsEnabled = _currentOrder.Items.Count > 0;
            ClearOrderButton.IsEnabled = _currentOrder.Items.Count > 0;
        }

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

            // Create receipt
            var receipt = _receiptFactory.CreateReceipt(_currentOrder);

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

        private void ClearOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrder.Items.Count == 0)
                return;

            var result = MessageBox.Show(
                "Clear the current order? All items will be removed.",
                "Confirm Clear Order",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                StartNewOrder();
                DiscountTextBox.Clear();
                StatusText.Text = "Order cleared";
            }
        }

        private void RefreshMenuButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeMenu();
            StatusText.Text = "Menu refreshed";
        }

        private void StartNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrder.Items.Count > 0)
            {
                var result = MessageBox.Show(
                    "Starting a new order will clear the current order. Continue?",
                    "Confirm New Order",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            StartNewOrder();
        }
    }
}