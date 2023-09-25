using Lab2.Interfaces;
using Lab2.Models;
using Lab2.Utils;
using Newtonsoft.Json;
using System.IO;

namespace Lab2
{
    internal class StoreConfig : IStoreConfig
    {
        public string StoreName { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public List<SeedCategoryProducts> Products { get; set; }
        public CurrencyType DataBaseCurrency { get; set; }
        public Dictionary<string, string> FilePaths { get; set; }

        public StoreConfig(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var storeDto = JsonConvert.DeserializeObject<StoreConfigDTO>(jsonData);

            StoreName = storeDto.StoreName;
            LinkedInUrl = storeDto.LinkedInUrl;
            GitHubUrl = storeDto.GitHubUrl;
            Products = storeDto.ProductsSeed;
            DataBaseCurrency = Enum.Parse<CurrencyType>(storeDto.DataBaseCurrencyString, true);
            FilePaths = storeDto.FilePaths;
        }
    }
}