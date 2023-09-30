using Lab2.Models;
using Lab2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.ShopAI
{
    internal static class BotDatabaseAccess
    {
        public static IEnumerable<Product> StoreProducts { get; set; }
       
        static BotDatabaseAccess()
        {
            StoreProducts = ProductService.GetAllProducts();
        }

    }
}
