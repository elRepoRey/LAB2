using System;
using System.Collections.Generic;
using System.Linq;
using Lab2.Models;
using Lab2.Interfaces;

namespace Lab2.Services
{
    internal class ProductService
    {
        private readonly DatabaseService<ProductDAO> _productDbService;
        private readonly ICurrencyServices _currencyManager;
        private List<Product> _products;

        public ProductService(DatabaseService<ProductDAO> productDbService, ICurrencyServices currencyManager)
        {
            _productDbService = productDbService ?? throw new ArgumentNullException(nameof(productDbService));
            _currencyManager = currencyManager ?? throw new ArgumentNullException(nameof(currencyManager));
            _products = LoadProducts();
        }

        private List<Product> LoadProducts()
        {
            try
            {
                var productDAOs = _productDbService.LoadDataFromFiles();
                return productDAOs.Select(p => new Product(p.Name, ConvertToGlobalPrice(p.Price), p.Category)).ToList();
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        private decimal ConvertToGlobalPrice(decimal priceInDbCurrency)
        {
            return _currencyManager.ConvertToGlobalCurrency(priceInDbCurrency);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public IEnumerable<string> GetDistinctCategories()
        {
            return _products.Select(p => p.Category).Distinct();
        }
    }
}
