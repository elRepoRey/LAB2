namespace Lab2.Models
{
    internal class StoreConfigDTO
    {
        public string StoreName { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string DataBaseCurrencyString { get; set; }
        public Dictionary<string, string> FilePaths { get; set; }

        public List<CustomerDAO> CustomersSeed { get; set; } 
        public List<SeedCategoryProducts> ProductsSeed { get; set; }
    }
}
