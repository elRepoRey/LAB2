using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;
using System.Linq;

namespace Lab2.Views
{
    internal static class Settings
    {      
        public static void Render()
        {
            CurrencyType[] availableCurrencies = Enum.GetValues(typeof(CurrencyType))
                                                     .Cast<CurrencyType>()
                                                     .ToArray();

            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Body.Clear();
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition($"Current currency: {CurrencyServices.GetCurrencySymbol()}".Length), 3);
                Console.Write("Current currency: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{CurrencyServices.GetCurrencySymbol()}");
                Console.ResetColor();
                Console.WriteLine();

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition("Select a new currency from the list:".Length), 5);
                Console.WriteLine("Select a new currency from the list:");

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition("==================================".Length), 6);
                Console.WriteLine("==================================");

                for (int i = 0; i < availableCurrencies.Length; i++)
                {
                    string currencyOption = $"{i + 1}. {availableCurrencies[i]} {CurrencyServices.GetCurrencySymbol(availableCurrencies[i])}";
                    Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(currencyOption.Length), 7 + i);

                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(currencyOption);
                    Console.ResetColor();
                }

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition("==================================".Length), 7 + availableCurrencies.Length);
                Console.WriteLine("==================================");

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedIndex < availableCurrencies.Length - 1)
                            selectedIndex++;
                        break;

                    case ConsoleKey.Enter:                       
                        ChangeCurrency(availableCurrencies[selectedIndex]);
                        
                        break;
                    default:
                        break;
                }
            } while (key != ConsoleKey.Escape);
        } 

        private static void ChangeCurrency(CurrencyType newCurrency)
        {
            CurrencyServices.SetGlobalCurrency(newCurrency);          
            Navbar.UpdateMenuItems();           
        }
    }
}
