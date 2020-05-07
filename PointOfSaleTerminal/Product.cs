using EnsureThat;

namespace PointOfSaleTerminal
{
    public interface IProduct
    {
        public string Code { get; }
        public decimal UnitPrice { get; }

        public decimal GetDiscountPrice(int quantity, int discountPercent);

        public sealed decimal GetNonDiscountPrice(int quantity)
        {
            return quantity * UnitPrice;
        }
    }

    public class Product : IProduct
    {
        public string Code { get; }
        public decimal UnitPrice { get; }

        public Product(string code, decimal unitPrice)
        {
            Ensure.That(code).IsNotNull();
            Ensure.That(unitPrice).IsGt(0m);

            Code = code;
            UnitPrice = unitPrice;
        }

        public decimal GetDiscountPrice(int quantity, int discountPercent)
        {
            Ensure.That(quantity).IsGt(0);
            Ensure.That(discountPercent).IsGte(0);
            Ensure.That(discountPercent).IsLt(100);

            var price = quantity * UnitPrice;
            var discount = price * discountPercent / 100;

            return price - discount;
        }
    }

    public class VolumePricedProduct : IProduct
    {
        readonly decimal _volumePrice;
        readonly int _volumeSize;

        public string Code { get; }
        public decimal UnitPrice { get; }

        public VolumePricedProduct(string code, decimal unitPrice, decimal volumePrice, int volumeSize)
        {
            Ensure.That(code).IsNotNull();
            Ensure.That(unitPrice).IsGt(0m);
            Ensure.That(volumePrice).IsGt(0m);
            Ensure.That(volumeSize).IsGt(0);

            Code = code;
            UnitPrice = unitPrice;
            _volumePrice = volumePrice;
            _volumeSize = volumeSize;
        }

        public decimal GetDiscountPrice(int quantity, int discountPercent)
        {
            Ensure.That(quantity).IsGt(0);
            Ensure.That(discountPercent).IsGte(0);
            Ensure.That(discountPercent).IsLt(100);

            var totalVolumePrice = quantity / _volumeSize * _volumePrice;
            var totalUnitPrice = quantity % _volumeSize * UnitPrice;
            var discount = totalUnitPrice * discountPercent / 100;

            return totalVolumePrice + totalUnitPrice - discount;
        }
    }
}