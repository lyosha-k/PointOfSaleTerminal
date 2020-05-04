using Xunit;

namespace PointOfSaleTerminal.UnitTests
{
    public class PointOfSaleTerminalTests
    {
        [Theory]
        [InlineData("ABCDABA", 13.25)]
        [InlineData("CCCCCCC", 6.00)]
        [InlineData("ABCD", 7.25)]
        public void CalculateTotal_ReturnsCorrectResults(string productCodes, decimal expectedTotal)
        {
            var terminal = GetTerminal();
            
            foreach (var code in productCodes)
                terminal.Scan(code.ToString());

            var actual = terminal.CalculateTotal();

            Assert.Equal(expectedTotal, actual);
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