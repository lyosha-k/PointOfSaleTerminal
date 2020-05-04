using EnsureThat;

namespace PointOfSaleTerminal
{
    public interface IProduct
    {
        public string Code { get; }
        public decimal GetTotalPrice(int quantity);
    }

    public class Product : IProduct
    {
        readonly decimal _unitPrice;
        public string Code { get; }

        public Product(string code, decimal unitPrice)
        {
            Ensure.That(code).IsNotNull();
            Ensure.That(unitPrice).IsGt(0m);

            Code = code;
            _unitPrice = unitPrice;
        }

        public decimal GetTotalPrice(int quantity)
        {
            Ensure.That(quantity).IsGt(0);

            return quantity * _unitPrice;
        }
    }

    public class VolumePricedProduct : IProduct
    {
        readonly decimal _unitPrice;
        readonly decimal _volumePrice;
        readonly int _volumeSize;
        public string Code { get; }

        public VolumePricedProduct(string code, decimal unitPrice, decimal volumePrice, int volumeSize)
        {
            Ensure.That(code).IsNotNull();
            Ensure.That(unitPrice).IsGt(0m);
            Ensure.That(volumePrice).IsGt(0m);
            Ensure.That(volumeSize).IsGt(0);

            Code = code;
            _unitPrice = unitPrice;
            _volumePrice = volumePrice;
            _volumeSize = volumeSize;
        }

        public decimal GetTotalPrice(int quantity)
        {
            Ensure.That(quantity).IsGt(0);

            var volumesCount = quantity / _volumeSize;
            var remainingProducts = quantity % _volumeSize;

            return volumesCount * _volumePrice
                   + remainingProducts * _unitPrice;
        }
    }
}