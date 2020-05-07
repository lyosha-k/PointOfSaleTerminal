using EnsureThat;

namespace PointOfSaleTerminal
{
    public interface IProduct
    {
        public string Code { get; }

        public (decimal FullPrice, decimal DiscountPrice) CalculatePrice(int quantity, int discountPercent);
    }

    public class Product : IProduct
    {
        public string Code { get; }
        readonly decimal _unitPrice;

        public Product(string code, decimal unitPrice)
        {
            Ensure.That(code).IsNotNull();
            Ensure.That(unitPrice).IsGt(0m);

            Code = code;
            _unitPrice = unitPrice;
        }

        public (decimal FullPrice, decimal DiscountPrice) CalculatePrice(int quantity, int discountPercent)
        {
            Ensure.That(quantity).IsGt(0);
            Ensure.That(discountPercent).IsGte(0);
            Ensure.That(discountPercent).IsLt(100);

            var fullPrice = quantity * _unitPrice;
            var discount = fullPrice * discountPercent / 100;

            return (fullPrice, fullPrice - discount);
        }
    }

    public class VolumePricedProduct : IProduct
    {
        readonly decimal _volumePrice;
        readonly int _volumeSize;

        public string Code { get; }
        readonly decimal _unitPrice;

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

        public (decimal FullPrice, decimal DiscountPrice) CalculatePrice(int quantity, int discountPercent)
        {
            Ensure.That(quantity).IsGt(0);
            Ensure.That(discountPercent).IsGte(0);
            Ensure.That(discountPercent).IsLt(100);

            var fullPrice = quantity * _unitPrice;
            var discountPrice = GetDiscountPrice(quantity, discountPercent);

            return (fullPrice, discountPrice);
        }

        decimal GetDiscountPrice(int quantity, int discountPercent)
        {
            var totalVolumePrice = quantity / _volumeSize * _volumePrice;
            var totalUnitPrice = quantity % _volumeSize * _unitPrice;
            var discount = totalUnitPrice * discountPercent / 100;

            return totalVolumePrice + totalUnitPrice - discount;
        }
    }
}