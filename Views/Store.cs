using Lab2.Models;
using Lab2.Utils;
using Lab2.Views.Shared;
using Lab2.Services;
using System;
using System.Linq;


namespace Lab2.Views
{
    internal static class Store
    {           
        private static  int MainContentStartX = 19;
        private static int SelectedProductIndex = 0;

        public static void Render()
        {
            string selectedCategory = null;

            while (true)
            {
                
                if (selectedCategory == null)
                {
                    selectedCategory = Sidebar.Render();
                }

                if (selectedCategory == null)
                {
                    ClearMainContentArea();
                    break;  
                }
                
                if (!DisplayAndSelectProductsForCategory(selectedCategory))
                {
                    selectedCategory = null;
                    continue;
                }
            }       
        }

        private static bool DisplayAndSelectProductsForCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return false; 
            }

            var productsForCategory = ProductService.GetAllProducts().Where(p => p.Category == category).ToList();

            if (!productsForCategory.Any())
                return false; 
            
            while (true)
            {                
                int yPosition = 4;
                for (int i = 0; i < productsForCategory.Count; i++)
                {
                    Console.SetCursorPosition(MainContentStartX, yPosition);
                    if (i == SelectedProductIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    yPosition += 3;
                    Console.WriteLine($"{productsForCategory[i].Name} - {CurrencyServices.ConvertToGlobalCurrency(productsForCategory[i].Price)} {CurrencyServices.GlobalCurrency}");
                    Console.ResetColor();
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        SelectedProductIndex = (SelectedProductIndex - 1 + productsForCategory.Count) % productsForCategory.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        SelectedProductIndex = (SelectedProductIndex + 1) % productsForCategory.Count;
                        break;
                    case ConsoleKey.Enter:
                        if (GlobalState.LoggedInCustomer.Cart.Add(productsForCategory[SelectedProductIndex]))
                        {
                            Navbar.FlashCart(); 
                        }

                        break;
                    case ConsoleKey.Escape:

                        for (int i = 0; i < productsForCategory.Count; i++)
                        {
                            Console.SetCursorPosition(MainContentStartX, 3 + i * 2);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{productsForCategory[i].Name} - {productsForCategory[i].Price} SEK");
                        }
                        ClearMainContentArea();
                        SelectedProductIndex = 0;
                     return false;
                }
            }
        }

        private static void ClearMainContentArea()
        {
            for (int i = 3; i < Console.WindowHeight - 3; i++)
            {
                Console.SetCursorPosition(MainContentStartX, i);
                Console.Write(new string(' ', Console.WindowWidth - MainContentStartX));
            }
        }
    }
}
