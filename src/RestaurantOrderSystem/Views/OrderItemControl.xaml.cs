using System.Windows;
using System.Windows.Controls;
using ModelMenuItem = RestaurantOrderSystem.Models.MenuItem;

namespace RestaurantOrderSystem.Views
{
    public partial class OrderItemControl : UserControl
    {
        // Dependency Property for MenuItem (model)
        public static readonly DependencyProperty MenuItemProperty =
            DependencyProperty.Register("MenuItem", typeof(ModelMenuItem), typeof(OrderItemControl),
                new PropertyMetadata(null, OnMenuItemChanged));

        public ModelMenuItem MenuItem
        {
            get => (ModelMenuItem)GetValue(MenuItemProperty);
            set => SetValue(MenuItemProperty, value);
        }

        // Event for when item is removed (nullable to satisfy nullable-reference rules)
        public event RoutedEventHandler? RemoveClicked;

        public OrderItemControl()
        {
            InitializeComponent();
        }

        private static void OnMenuItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OrderItemControl)d;
            control.UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (MenuItem != null)
            {
                ItemNameText.Text = MenuItem.Name;
                ItemCategoryText.Text = MenuItem.Category;
                ItemPriceText.Text = $"${MenuItem.Price:F2}";
                RemoveButton.Tag = MenuItem.Id;
            }
            else
            {
                ItemNameText.Text = string.Empty;
                ItemCategoryText.Text = string.Empty;
                ItemPriceText.Text = string.Empty;
                RemoveButton.Tag = null;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveClicked?.Invoke(this, e);
        }
    }
}
