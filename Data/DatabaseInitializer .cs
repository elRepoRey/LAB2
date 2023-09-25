using Lab2.Interfaces;
using Lab2.Models;
using Lab2.Services;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Lab2.Data
{
    internal class DatabaseInitializer
    {
        private readonly IStoreConfig _storeConfig;
        private readonly DatabaseService<CustomerDAO> _customerDbService;
        private readonly DatabaseService<Product> _productDbService;

        public DatabaseInitializer(IStoreConfig storeConfig, DatabaseService<CustomerDAO> customerDbService, DatabaseService<Product> productDbService)
        {
            _storeConfig = storeConfig;
            _customerDbService = customerDbService;
            _productDbService = productDbService;
        }

        public void Initialize()
        {
            // Check if files already exist
            if (File.Exists(_storeConfig.FilePaths["Customers"]) && File.Exists(_storeConfig.FilePaths["Products"]))
            {
                // Data files already exist, skip initialization
                return;
            }

            // Use the StoreConfig to get the JSON data path
            var jsonData = File.ReadAllText("StoreConfigData.json");

            // Deserialize the JSON data
            var data = JsonConvert.DeserializeObject<Root>(jsonData);

            if (data == null)
            {
                throw new Exception("Failed to deserialize the configuration data.");
            }

            // Extract customers and products
            var customers = data.CustomersSeed
                .Select(c => new CustomerDAO(c.Name, c.Password))
                .ToList();

            var products = new List<Product>();

            if (data.ProductsSeed != null)
            {
                products = data.ProductsSeed
                    .Where(category => category.StoreItems != null) // Check for StoreItems instead of SeedProducts
                    .SelectMany(category => category.StoreItems
                        .Select(product => new Product(product.Name, product.Price, category.Category)))
                    .ToList();
            }

            // Save to JSON files using injected services
            _customerDbService.SaveDataToFiles(customers);
            _productDbService.SaveDataToFiles(products);
        }



       
        private class Root
        {
            public List<CustomerDAO> CustomersSeed { get; set; }
            public List<SeedCategoryProducts> ProductsSeed { get; set; }
        }



    }
}
