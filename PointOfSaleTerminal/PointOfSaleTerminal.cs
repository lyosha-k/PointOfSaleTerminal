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
        
        public void SetPricing(IEnumerable<IProduct> products)
        {
            Ensure.That(products).IsNotNull();

            _knownProducts = products.ToDictionary(p => p.Code);
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
            decimal total = 0;
            
            foreach (var (code, quantity) in _scannedProducts)
                total += _knownProducts[code].GetTotalPrice(quantity);
            
            return total;
        }
    }
}