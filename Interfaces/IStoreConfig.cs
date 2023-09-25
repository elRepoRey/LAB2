using Lab2.Models;
using Lab2.Utils;
using System.Collections.Generic;

namespace Lab2.Interfaces
{
    internal interface IStoreConfig
    {
        string StoreName { get; }
        string LinkedInUrl { get; }
        string GitHubUrl { get; }
        List<SeedCategoryProducts> Products { get; }
        CurrencyType DataBaseCurrency { get; }
        Dictionary<string,string> FilePaths { get; }
    }
}