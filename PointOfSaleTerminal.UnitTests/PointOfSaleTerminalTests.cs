using Moq;
using Xunit;

namespace PointOfSaleTerminal.UnitTests
{
    public class PointOfSaleTerminalTests
    {
        [Theory]
        [InlineData("ABCDABA", 13.25)]
        [InlineData("CCCCCCC", 6.00)]
        [InlineData("ABCD", 7.25)]
        public void CalculateTotal_WithNoDiscountCard_ReturnsCorrectResults(string productCodes, decimal expectedTotal)
        {
            // Arrange
            var sut = GetTerminal();

            foreach (var code in productCodes)
                sut.Scan(code.ToString());

            // Act
            var actual = sut.CalculateTotal();

            // Assert
            Assert.Equal(expectedTotal, actual);
        }

        [Theory]
        [InlineData(0, 13.25)]
        [InlineData(1000, 13.1475)]
        [InlineData(2000, 12.9425)]
        [InlineData(5000, 12.7375)]
        [InlineData(10000, 12.5325)]
        public void CalculateTotal_WithDiscountCard_AppliesCorrectDiscount(decimal discountCardAmount,
            decimal expectedTotal)
        {
            // Arrange
            var productCodes = "ABCDABA";

            var discountCard = new DiscountCard();
            discountCard.AddAmount(discountCardAmount);

            var sut = GetTerminal();
            sut.SetDiscountCard(discountCard);

            foreach (var code in productCodes)
                sut.Scan(code.ToString());

            // Act
            var actual = sut.CalculateTotal();

            // Assert
            Assert.Equal(expectedTotal, actual);
        }

        [Theory]
        [InlineData(0, 14)]
        [InlineData(1, 14)]
        [InlineData(3, 14)]
        [InlineData(5, 14)]
        [InlineData(7, 14)]
        public void CalculateTotal_WithDiscountCard_AddsFullAmount(int discountPercent, decimal expectedTotal)
        {
            // Arrange
            var productCodes = "ABCDABA";

            var discountCard = new Mock<IDiscountCard>();
            discountCard
                .Setup(c => c.GetDiscountPercent())
                .Returns(discountPercent);

            var sut = GetTerminal();
            sut.SetDiscountCard(discountCard.Object);

            foreach (var code in productCodes)
                sut.Scan(code.ToString());

            // Act
            sut.FinishSale();

            // Assert
            discountCard.Verify(dc => dc.AddAmount(expectedTotal));
        }

        PointOfSaleTerminal GetTerminal()
        {
            var terminal = new PointOfSaleTerminal();

            terminal.SetPricing(new IProduct[]
            {
                new VolumePricedProduct("A", 1.25m, 3.0m, 3),
                new Product("B", 4.25m),
                new VolumePricedProduct("C", 1.0m, 5.0m, 6),
                new Product("D", 0.75m),
            });

            return terminal;
        }
    }
}