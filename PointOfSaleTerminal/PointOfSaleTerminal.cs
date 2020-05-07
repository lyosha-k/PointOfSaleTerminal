using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace PointOfSaleTerminal
{
    public class PointOfSaleTerminal
    {
        Dictionary<string, IProduct> _knownProducts = new Dictionary<string, IProduct>();
        readonly Dictionary<string, int> _scannedProducts = new Dictionary<string, int>();
        IDiscountCard _discountCard = DiscountCard.None;

        public void SetPricing(IEnumerable<IProduct> products)
        {
            Ensure.That(products).IsNotNull();

            _knownProducts = products.ToDictionary(p => p.Code);
        }

        public void SetDiscountCard(IDiscountCard discountCard)
        {
            Ensure.That(discountCard).IsNotNull();

            _discountCard = discountCard;
        }

        public void Scan(string productCode)
        {
            Ensure.That(productCode).IsNotNull();

            if (!_knownProducts.ContainsKey(productCode))
                throw new ArgumentException($"Unknown product code: {productCode}");

            if (_scannedProducts.TryGetValue(productCode, out int quantity))
                _scannedProducts[productCode] = quantity + 1;
            else
                _scannedProducts[productCode] = 1;
        }

        public decimal CalculateTotal()
        {
            var total = 0m;
            var discountPercent = _discountCard.GetDiscountPercent();

            foreach (var (code, quantity) in _scannedProducts)
                total += _knownProducts[code].CalculatePrice(quantity, discountPercent).DiscountPrice;

            return total;
        }

        public void FinishSale()
        {
            var totalWithoutDiscount = 0m;

            foreach (var (code, quantity) in _scannedProducts)
                totalWithoutDiscount += _knownProducts[code].CalculatePrice(quantity, 0).FullPrice;

            _discountCard.AddAmount(totalWithoutDiscount);

            _discountCard = DiscountCard.None;
            _scannedProducts.Clear();
        }
    }
}