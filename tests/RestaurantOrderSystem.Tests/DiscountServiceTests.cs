using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;

namespace RestaurantOrderSystem.Tests
{
    public class DiscountServiceTests
    {
        [Fact]
        public void GetDiscount_ValidCode_ShouldReturnDiscount()
        {
            // Arrange
            var service = new DiscountService();
            var discounts = new List<Discount>
            {
                new Discount { Code = "SAVE10", Value = 10 }
            };

            // Act
            var result = service.GetDiscount("SAVE10", discounts);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SAVE10", result.Code);
        }

        [Fact]
        public void GetDiscount_InvalidCode_ShouldReturnNull()
        {
            // Arrange
            var service = new DiscountService();
            var discounts = new List<Discount>
            {
                new Discount { Code = "SAVE10", Value = 10 }
            };

            // Act
            var result = service.GetDiscount("INVALID", discounts);

            // Assert
            Assert.Null(result);
        }
    }
}
