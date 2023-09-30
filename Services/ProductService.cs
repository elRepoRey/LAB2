using System;
using System.Collections.Generic;
using System.Linq;
using Lab2.Models;
using Lab2.Services;

namespace Lab2.Services
{
    internal static class ProductService
    {
        
        private static List<Product> _products;

        static ProductService()
        {
          
            _products = LoadProducts();
        }

        private static List<Product> LoadProducts()
        {
            try
            {
                var productDAOs = DatabaseService<ProductDAO>.LoadDataFromFiles(StoreConfig.FilePaths["Products"]);
                return productDAOs.Select(p => new Product(p.Name, ( p.Price), p.Category)).ToList();
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public static IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public static IEnumerable<string> GetDistinctCategories()
        {
            return _products.Select(p => p.Category).Distinct();
        }
    }
}
