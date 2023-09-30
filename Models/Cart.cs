
using Lab2.Utils;
using System.Collections.Generic;
using System.Linq;
using Lab2.Services;
using Lab2.Views.Shared;

namespace Lab2.Models
{    public class Cart 
    {
        private const int MaxDistinctProducts = 8;  
            
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal TotalPrice => CartItems.Sum(ci => ci.TotalPrice);

        public int Count => CartItems.Sum(ci => ci.Quantity);

        public bool Add(Product product)
        {
            if (CartItems.Count >= MaxDistinctProducts && !CartItems.Any(ci => ci.Product.Name == product.Name))
            {
                Notification.Show($"{MaxDistinctProducts}", "items is the maximum number of distinct products" +
                    " allowed in the cart.", NotificationType.Error, 4000);
                return false;
            }

            var existingCartItem = CartItems.FirstOrDefault(ci => ci.Product.Name == product.Name);
            if (existingCartItem == null)
            {
                CartItems.Add(new CartItem(product));
                Navbar.UpdateMenuItems();
                return true;
            }
            else
            {
                existingCartItem.IncrementQuantity();
                Navbar.UpdateMenuItems();
                return true;
            }
        }

        public void Remove(Product product)
        {
            var existingCartItem = CartItems.FirstOrDefault(ci => ci.Product.Name == product.Name);
            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity > 1)
                {
                    existingCartItem.Quantity--;
                    Navbar.UpdateMenuItems();
                }
                else
                {
                    CartItems.Remove(existingCartItem);
                    Navbar.UpdateMenuItems();
                }                
            }
        }

        public void Clear()
        {
            CartItems.Clear();
            Navbar.UpdateMenuItems();
        }

    }

    public class CartItem 
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Product.Price * Quantity;

        public CartItem(Product product)
        {
            Product = product;
            Quantity = 1;
        }

        public void IncrementQuantity()
        {
            Quantity++;
            Navbar.UpdateMenuItems();

        }
    }

}
