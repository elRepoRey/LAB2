using Lab2.Utils;
using Newtonsoft.Json;
using System.IO;

namespace Lab2.Models
{
    internal static class StoreConfig
    {
        public static string StoreName { get; set; }
        public static string LinkedInUrl { get; set; }
        public static string GitHubUrl { get; set; }
        public static List<CategoryProducts> Products { get; set; }
        public static CurrencyType DataBaseCurrency { get; set; }
        public static Dictionary<string, string> FilePaths { get; set; }

        static StoreConfig()
        {
            var jsonData = File.ReadAllText("StoreSeedData.json");
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