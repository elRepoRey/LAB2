using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;
using System.Linq;

namespace Lab2.Views
{
    internal static class CheckoutCart
    {
        private static Cart _cart;
        private static int _selectedItemIndex = 0;
        private static int _selectedButtonIndex = 0;
        private static bool _checkoutSelected = false;

        static CheckoutCart()
        {
        }

        public static void Render()
        {
            _cart = GlobalState.LoggedInCustomer!.Cart;

            string Line = "+=======================================================+";
            string Columns = "|  Product Name | Quantity  |  Total Price  |    Ops    |";

            while (true)
            {
                Body.Clear();
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition("Your Cart:".Length), 4);
                Console.WriteLine("Your Cart:");
                Console.WriteLine();
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(Line.Length), 6);
                Console.WriteLine(Line);
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(Columns.Length), 7);
                Console.WriteLine(Columns);
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(Line.Length), 8);
                Console.WriteLine(Line);

                for (int i = 0; i < _cart.CartItems.Count; i++)
                {
                    var cartItem = _cart.CartItems[i];
                    string CartItemContent = $"{cartItem.Product.Name,-15} | {cartItem.Quantity,-8} | {CurrencyServices.GlobalCurrency} {CurrencyServices.ConvertToGlobalCurrency(cartItem.TotalPrice),-11} | ";
                    string CartItem = "| " + CartItemContent;

                    string plusButton = "[+]";
                    string minusButton = "[-]";

                    Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(CartItem.Length + plusButton.Length + minusButton.Length + 2), 9 + i);
                    Console.Write(CartItem);

                    if (i == _selectedItemIndex && !_checkoutSelected)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = _selectedButtonIndex == 0 ? ConsoleColor.DarkBlue : ConsoleColor.Black;
                        Console.Write(minusButton);
                        Console.ResetColor();

                        Console.Write(" ");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = _selectedButtonIndex == 1 ? ConsoleColor.DarkBlue : ConsoleColor.Black;
                        Console.Write(plusButton);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(minusButton);
                        Console.ResetColor();
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(plusButton);
                        Console.ResetColor();
                    }
                    Console.WriteLine("|");
                }

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(Line.Length), 9 + _cart.CartItems.Count);
                Console.WriteLine(Line);

                string Tier = GlobalState.LoggedInCustomer.Tier;
                decimal discount = GlobalState.LoggedInCustomer.DiscountRate;
                decimal discountedPrice = _cart.TotalPrice - (_cart.TotalPrice * discount);

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition($"Total Cart Price: {CurrencyServices.GlobalCurrency} {CurrencyServices.ConvertToGlobalCurrency(_cart.TotalPrice),-11}".Length), 11 + _cart.CartItems.Count);
                Console.WriteLine($"Total Cart Price: {CurrencyServices.GlobalCurrency} {CurrencyServices.ConvertToGlobalCurrency(_cart.TotalPrice),-11}");

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition($"{Tier} discount rate: {discount * 100}%".Length), 12 + _cart.CartItems.Count);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{Tier}");
                Console.ResetColor();
                Console.WriteLine($" discount rate: {discount * 100}%");

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition($"Discounted Price: {CurrencyServices.GlobalCurrency} {CurrencyServices.ConvertToGlobalCurrency(discountedPrice),-11}".Length), 13 + _cart.CartItems.Count);
                Console.WriteLine($"Discounted Price: {CurrencyServices.GlobalCurrency} {CurrencyServices.ConvertToGlobalCurrency(discountedPrice),-11}");

                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition(Line.Length), 14 + _cart.CartItems.Count);
                Console.WriteLine(Line);

                // Display Checkout button
                Console.SetCursorPosition(UiHelper.GetCenteredLeftPosition("[Checkout]".Length), 16 + _cart.CartItems.Count);

                if (_checkoutSelected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[Checkout]");
                    Console.ResetColor();
                }
                else
                {   
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("[Checkout]");
                    Console.ResetColor();
                }

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
                            if (GlobalState.LoggedInCustomer.Cart.CartItems.Count == 0)
                            {
                                Notification.Show("Your cart is empty.", "Add some items to your cart to continue.", NotificationType.Error, 4000);
                                break;
                            }
                            CustomerServices.Checkout();    
                                
                                Body.Clear();

                                string thankYouMessage = "Thank you for your purchase! Your cart has been cleared.";
                                string pressAnyKeyMessage = "Press any key to continue...";
                                int width = Math.Max(thankYouMessage.Length, pressAnyKeyMessage.Length) + 4;  

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
                        else if (_selectedItemIndex >= 0 && _selectedItemIndex < _cart.CartItems.Count)
                        {
                            if (_selectedButtonIndex == 1)
                            {
                                _cart.CartItems[_selectedItemIndex].IncrementQuantity();
                                

                            }
                            else
                            {
                                _cart.Remove(_cart.CartItems[_selectedItemIndex].Product);
                                _selectedItemIndex = Math.Min(_selectedItemIndex, _cart.CartItems.Count - 1);
                               

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
