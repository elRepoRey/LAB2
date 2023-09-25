using Lab2.Interfaces;
using Lab2.Utils;
using System.Collections.Generic;
using System.Linq;
using Lab2.Services;

namespace Lab2.Models
{    public class Cart 
    {
        private const int MaxDistinctProducts = 8;  // Maximum number of distinct products the cart can hold
        private readonly Notification _notification = new Notification();       

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal TotalPrice => CartItems.Sum(ci => ci.TotalPrice);


        public event Action? OnCartChanged;
        public int Count => CartItems.Sum(ci => ci.Quantity);

        public bool Add(Product product)
        {
            if (CartItems.Count >= MaxDistinctProducts && !CartItems.Any(ci => ci.Product.Name == product.Name))
            {
                _notification.Show($"{MaxDistinctProducts}", "items is the maximum number of distinct products" +
                    " allowed in the cart.", NotificationType.Error, 4000);
                return false;
            }

            var existingCartItem = CartItems.FirstOrDefault(ci => ci.Product.Name == product.Name);
            if (existingCartItem == null)
            {
                CartItems.Add(new CartItem(product));
                OnCartChanged?.Invoke();
                return true;
            }
            else
            {
                existingCartItem.IncrementQuantity();
                OnCartChanged?.Invoke();
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
                }
                else
                {
                    CartItems.Remove(existingCartItem);
                    OnCartChanged?.Invoke();
                }
                
            }
        }

        public void Clear()
        {
            CartItems.Clear();
            OnCartChanged?.Invoke();
        }
    }

    public class CartItem 
    {
        public event Action? OnCartChanged;
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
            OnCartChanged?.Invoke();

        }
    }
}
