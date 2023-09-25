using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Models
{
    internal class SeedCategoryProducts
    {
        public string Category { get; set; }
        public List<Product> StoreItems { get; set; }
    }
}
