using Lab2.Interfaces;
using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;

namespace Lab2.Views
{
    internal class Settings 
    {
        public CurrencyType CurrentCurrency { get; private set; } = CurrencyType.SEK; // Default currency
        private readonly ICurrencyServices _currencyManager;
        private readonly Notification _notification;
        private readonly Navbar _navbar;

        public Settings(ICurrencyServices currencyManager, Notification notification, Navbar navbar)
        {
            _currencyManager = currencyManager;
            _notification = notification;
            _navbar = navbar;
        } 

        public void DisplayAndChangeCurrency()
        {
            CurrencyType[] availableCurrencies = Enum.GetValues(typeof(CurrencyType))
                                                     .Cast<CurrencyType>()
                                                     .ToArray();

            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.SetCursorPosition( 0, 3);
                Console.Write($"Current currency:");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($" {_currencyManager.GetCurrencySymbol(CurrentCurrency)}");
                Console.ResetColor();
                Console.WriteLine();

                Console.SetCursorPosition(0, 5); // Move cursor up one line
                Console.WriteLine("Select a new currency from the list:");
               
                Console.WriteLine("==================================");
                for (int i = 0; i < availableCurrencies.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine($"{i + 1}. {availableCurrencies[i]} {_currencyManager.GetCurrencySymbol(availableCurrencies[i])}");
                    Console.ResetColor();
                }
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

        private void ChangeCurrency(CurrencyType newCurrency)
        {
            CurrentCurrency = newCurrency;
            _currencyManager.SetGlobalCurrency(newCurrency);
            _navbar.UpdateMenuItems();
            _notification.Show( $"{_currencyManager.GetCurrencySymbol(newCurrency)}", "has been set as the new currency.", NotificationType.Success);
        }
    }
    }
