using Lab2.Interfaces;
using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;
using System.Linq;

namespace Lab2.Views
{
    internal class CheckoutCart
    {
        private  Cart _cart;
        private int _selectedItemIndex = 0;
        private int _selectedButtonIndex = 0;  // 0 for '+', 1 for '-'
        private bool _checkoutSelected = false;  // Flag to determine if checkout button is selected
        private readonly ICurrencyServices _currencyManager;
        private readonly CustomerServices _customerServices;
        public event Action? OnCartChanged;
        
        
        
        public CheckoutCart(ICurrencyServices currencyManager, CustomerServices customerServices)
        {
            _currencyManager = currencyManager;           
            _customerServices = customerServices;
        }

        public void Render()
        {
            _cart = GlobalState.LoggedInCustomer.Cart;
            

            while (true)
            {
                Console.SetCursorPosition(0, 4);
                Console.WriteLine("Your Cart:");
                Console.WriteLine();
                Console.WriteLine("============================================");
                Console.WriteLine("| Product Name | Quantity | Total Price | Ops |");
                Console.WriteLine("============================================");

                for (int i = 0; i < _cart.CartItems.Count; i++)
                {
                    var cartItem = _cart.CartItems[i];
                    Console.Write($"| {cartItem.Product.Name,-15} | {cartItem.Quantity,-8} | {_currencyManager.GlobalCurrency} {_currencyManager.ConvertToGlobalCurrency(cartItem.TotalPrice),-11} | ");

                    string plusButton = "[+]";
                    string minusButton = "[-]";

                    if (i == _selectedItemIndex && !_checkoutSelected)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = _selectedButtonIndex == 0 ? ConsoleColor.DarkBlue : ConsoleColor.Black;
                        Console.Write(plusButton);
                        Console.ResetColor();

                        Console.Write(" ");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = _selectedButtonIndex == 1 ? ConsoleColor.DarkBlue : ConsoleColor.Black;
                        Console.Write(minusButton);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(plusButton);
                        Console.ResetColor();
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(minusButton);
                        Console.ResetColor();
                    }
                    Console.WriteLine("|");
                }
                string Tier = GlobalState.LoggedInCustomer.Tier;
                decimal discount = GlobalState.LoggedInCustomer.DiscountRate;
                decimal discountedPrice = _cart.TotalPrice - (_cart.TotalPrice * discount);
                Console.WriteLine("============================================");
                Console.WriteLine($"Total Cart Price: { _currencyManager.GlobalCurrency} {_currencyManager.ConvertToGlobalCurrency(_cart.TotalPrice),-11}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{Tier}");
                Console.ResetColor();
                Console.Write($" discount rate: {discount * 100}%");
                Console.WriteLine();
                Console.WriteLine($"Discounted Price: {_currencyManager.GlobalCurrency} {_currencyManager.ConvertToGlobalCurrency(discountedPrice),-11}");
                Console.WriteLine("============================================");


                // Display Checkout button

                if (_checkoutSelected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
                Console.WriteLine("[Checkout]");
                Console.ResetColor();



                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_checkoutSelected)
                        {
                            _checkoutSelected = false;
                        }
                        else if (_selectedItemIndex > 0)
                        {
                            _selectedItemIndex--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (_selectedItemIndex < _cart.CartItems.Count - 1)
                        {
                            _selectedItemIndex++;
                        }
                        else
                        {
                            _checkoutSelected = true;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        _selectedButtonIndex = 0;
                        break;

                    case ConsoleKey.Escape:
                        return;

                    case ConsoleKey.RightArrow:
                        _selectedButtonIndex = 1;
                        break;

                    case ConsoleKey.Enter:
                       
                        Body.Clear();
                        if (_checkoutSelected)
                        {
                            {
                                _customerServices.Checkout();                                
                                Body.Clear();

                                string thankYouMessage = "Thank you for your purchase! Your cart has been cleared.";
                                string pressAnyKeyMessage = "Press any key to continue...";
                                int width = Math.Max(thankYouMessage.Length, pressAnyKeyMessage.Length) + 4;  // +4 for padding

                                Console.SetCursorPosition((Console.WindowWidth - width) / 2, Console.WindowHeight / 4);
                                Console.WriteLine(new string('=', width));

                                Console.SetCursorPosition((Console.WindowWidth - thankYouMessage.Length) / 2, Console.WindowHeight / 4 + 1);
                                Console.WriteLine(thankYouMessage);

                                Console.SetCursorPosition((Console.WindowWidth - pressAnyKeyMessage.Length) / 2, Console.WindowHeight / 4 + 2);
                                Console.WriteLine(pressAnyKeyMessage);

                                Console.SetCursorPosition((Console.WindowWidth - width) / 2, Console.WindowHeight / 4 + 3);
                                Console.WriteLine(new string('=', width));

                                Console.ReadKey();
                                return;
                            }

                        }
                        else if (_selectedItemIndex >= 0 && _selectedItemIndex < _cart.CartItems.Count)
                        {
                            if (_selectedButtonIndex == 0)
                            {
                                _cart.CartItems[_selectedItemIndex].IncrementQuantity();
                                OnCartChanged?.Invoke();
                            }
                            else
                            {
                                _cart.Remove(_cart.CartItems[_selectedItemIndex].Product);
                                _selectedItemIndex = Math.Min(_selectedItemIndex, _cart.CartItems.Count - 1);
                                OnCartChanged?.Invoke();
                            }
                        }
                        break;

                    case ConsoleKey.X:
                        return;
                }

            }
        }
    }
}
