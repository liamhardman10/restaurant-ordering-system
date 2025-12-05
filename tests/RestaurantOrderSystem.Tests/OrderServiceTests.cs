using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;

namespace RestaurantOrderSystem.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder_ShouldIncrementOrderId()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var order1 = service.CreateOrder();
            var order2 = service.CreateOrder();

            // Assert
            Assert.Equal(1, order1.OrderId);
            Assert.Equal(2, order2.OrderId);
        }

        [Fact]
        public void CalculateSubtotal_ShouldSumItemPrices()
        {
            // Arrange
            var service = new OrderService();
            var order = service.CreateOrder();
            var item1 = new MenuItem(1, "Burger", 8.99m, "Main");
            var item2 = new MenuItem(2, "Fries", 3.99m, "Side");

            // Act
            service.AddItemToOrder(order, item1);
            service.AddItemToOrder(order, item2);
            var subtotal = service.CalculateSubtotal(order);

            // Assert
            Assert.Equal(12.98m, subtotal);
        }
    }
}
