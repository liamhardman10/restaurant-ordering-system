using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;

namespace RestaurantOrderSystem.Tests
{
    public class PricingServiceTests
    {
        [Fact]
        public void CalculateTax_ShouldApplyTaxRate()
        {
            // Arrange
            var service = new PricingService();
            decimal amount = 100m;

            // Act
            var tax = service.CalculateTax(amount);

            // Assert
            Assert.Equal(8m, tax); // 8% of 100
        }

        [Fact]
        public void ApplyDiscount_Percentage_ShouldReduceAmount()
        {
            // Arrange
            var service = new PricingService();
            decimal amount = 100m;
            var discount = new Discount
            {
                Type = DiscountType.Percentage,
                Value = 10
            };

            // Act
            var result = service.ApplyDiscount(amount, discount);

            // Assert
            Assert.Equal(90m, result);
        }

        [Fact]
        public void ApplyDiscount_FixedAmount_ShouldSubtractValue()
        {
            // Arrange
            var service = new PricingService();
            decimal amount = 100m;
            var discount = new Discount
            {
                Type = DiscountType.FixedAmount,
                Value = 15
            };

            // Act
            var result = service.ApplyDiscount(amount, discount);

            // Assert
            Assert.Equal(85m, result);
        }
    }
}
