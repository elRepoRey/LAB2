using Lab2.Models;
using Lab2.Utils;
using Lab2.Views.Shared;
using Lab2.Services;
using System;
using System.Linq;
using Lab2.Interfaces;

namespace Lab2.Views
{
    internal class Store
    {
        private readonly ProductService _productService;
        private readonly ICurrencyServices _currencyManager;
        private readonly Sidebar _sidebar;
        private readonly Notification _notification;     
        private const int MainContentStartX = 19;
        private int _selectedProductIndex = 0;
        private Navbar _navbar;

        public Store(ProductService productService, Sidebar sidebar, Notification notification, Navbar navbar, ICurrencyServices currencyManager)
        {
            _productService = productService;
            _sidebar = sidebar;
            _notification = notification;
            _navbar = navbar;
            _currencyManager = currencyManager;
           
            
        }

        public void Render()
        {
            string selectedCategory = null;

            while (true)
            {
                // Only render the sidebar if there's no current category selected
                if (selectedCategory == null)
                {
                    selectedCategory = _sidebar.Render();
                }

                if (selectedCategory == null)
                {
                    ClearMainContentArea();
                    break;  // Exit the store if no category is selected
                }

                // If we're escaping from the products view, reset the category
                if (!DisplayAndSelectProductsForCategory(selectedCategory))
                {
                    selectedCategory = null;
                    continue;
                }
            }
        }


        private bool DisplayAndSelectProductsForCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return false; // Exit early if category is null or empty
            }

            var productsForCategory = _productService.GetAllProducts().Where(p => p.Category == category).ToList();

            if (!productsForCategory.Any())
                return false; // Exit if category doesn't exist or has no products

            ConsoleKey key;
            while (true)
            {
                // Display products
                int yPosition = 4;
                for (int i = 0; i < productsForCategory.Count; i++)
                {
                    Console.SetCursorPosition(MainContentStartX, yPosition);
                    if (i == _selectedProductIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    yPosition += 3;
                    Console.WriteLine($"{productsForCategory[i].Name} - {_currencyManager.ConvertToGlobalCurrency(productsForCategory[i].Price)} {_currencyManager.GlobalCurrency}");
                    Console.ResetColor();
                }

                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedProductIndex = (_selectedProductIndex - 1 + productsForCategory.Count) % productsForCategory.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedProductIndex = (_selectedProductIndex + 1) % productsForCategory.Count;
                        break;
                    case ConsoleKey.Enter:
                        if (GlobalState.LoggedInCustomer.Cart.Add(productsForCategory[_selectedProductIndex]))
                        {
                            _notification.Show($"{productsForCategory[_selectedProductIndex].Name}", $" was added to your cart", NotificationType.Success);
                            
                            _navbar.UpdateMenuItems();
                           
                        }

                        break;
                    case ConsoleKey.Escape: // Escape from products view, return to sidebar view and clear 
                        
                           for (int i = 0; i < productsForCategory.Count; i++)
                        {
                            Console.SetCursorPosition(MainContentStartX, 3 + i * 2);
                            Console.BackgroundColor = ConsoleColor.Black; // Reset background color
                            Console.ForegroundColor = ConsoleColor.White; // Reset foreground color
                            Console.WriteLine($"{productsForCategory[i].Name} - {productsForCategory[i].Price} SEK");
                        }
                        ClearMainContentArea();
                        _selectedProductIndex = 0;  
                        return false;

                }
            }
        }


        private void ClearMainContentArea()
        {
            for (int i = 3; i < Console.WindowHeight - 3; i++)
            {
                Console.SetCursorPosition(MainContentStartX, i);
                Console.Write(new string(' ', Console.WindowWidth - MainContentStartX));
            }
        }
    }
}
