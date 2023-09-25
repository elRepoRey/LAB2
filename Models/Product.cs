using Lab2.Models;
using Lab2.Utils;

namespace Lab2.Models;
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
   
    public Product(string name, decimal price, string category)
    {
        Name = name;
        Price = price;
        Category = category;
    }

    public override string ToString() => $"{Name} - {Price}";
}