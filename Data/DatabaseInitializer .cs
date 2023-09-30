using Lab2.Models;
using Lab2.Services;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Lab2.Data
{
    internal class DatabaseInitializer
    {
        public DatabaseInitializer()
        {
        }

        public void Initialize()
        {
            if (FilesExistAndNotEmpty(StoreConfig.FilePaths["Customers"], StoreConfig.FilePaths["Products"]))
            {
                return;
            }

            var jsonData = File.ReadAllText("StoreSeedData.json");

            var data = JsonConvert.DeserializeObject<Root>(jsonData);

            if (data == null)
            {
                throw new Exception("Failed to deserialize the configuration data.");
            }

            var customers = data.CustomersSeed
                .Select(c => new CustomerDAO(c.Name, c.Password))
                .ToList();


             var products = data.ProductsSeed
              .Where(category => category.Products != null)
              .SelectMany(category => category.Products
                  .Select(product => new ProductDAO(product.Name, product.Price, category.Category)))
              .ToList();

            DatabaseService<CustomerDAO>.SaveDataToFiles(customers, StoreConfig.FilePaths["Customers"]);
            DatabaseService<ProductDAO>.SaveDataToFiles(products, StoreConfig.FilePaths["Products"]);
        }

        private bool FilesExistAndNotEmpty(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private class Root
        {
            public List<CustomerDAO> CustomersSeed { get; set; }
            public List<CategoryProducts> ProductsSeed { get; set; }
        }
       

    }
}
