using Lab2.Interfaces;
using Lab2.Services;
using Lab2.Utils;
using System;
using System.Collections.Generic;

namespace Lab2.Views.Shared
{
    internal class Navbar
    {   private ICurrencyServices _currencyManager;
        private string _cartView;
        private List<string> _menuItems;
        private int _selectedItemIndex = 0;
        private int _navbarStartLine = 0;
        private decimal _cartTotal;
        public int _navbarEndLine => _navbarStartLine + 2;
        private CheckoutCart _checkoutcart;

        private string _customerView;

        public Navbar(ICurrencyServices currencyManager, CheckoutCart checkoutCart)
        {
            _currencyManager = currencyManager;
            _checkoutcart = checkoutCart;

            // Subscribe to login change events
            GlobalState.OnLoginChanged += (customer) =>
            {
                // Subscribe to the new customer's cart
                if (customer?.Cart != null)
                {
                    customer.Cart.OnCartChanged += UpdateCartView;
                    _currencyManager.OnCurrencyChanged += UpdateCartView;
                    _checkoutcart.OnCartChanged += UpdateCartView;
                }             

                
            };            
            
            UpdateCartView();
        }

        private void UpdateCartView()
        {
            _cartTotal = _currencyManager.ConvertToGlobalCurrency(GlobalState.LoggedInCustomer?.Cart.TotalPrice ?? 0);

            _cartView = $"{EnumNavBar.Cart} |{GlobalState.LoggedInCustomer?.Cart.Count ?? 0}| |{_currencyManager.GlobalCurrency} {_cartTotal}|";


        }


        public void UpdateMenuItems()
        {

            Clear();
            _menuItems = new List<string>
            {
                EnumNavBar.MyCornerStore.ToString()
            };


            if (GlobalState.LoggedInCustomer != null)
            {
                _customerView = $"{EnumNavBar.Welcome} {GlobalState.LoggedInCustomer.Name} |{GlobalState.LoggedInCustomer.Tier}|";
                _menuItems.AddRange(new List<string>
                {
                   
                    EnumNavBar.Settings.ToString(),
                     _cartView,
                    _customerView,
                    EnumNavBar.Logout.ToString()
                });
            }


            Render();
        }

        public void Render()
        {
            // Set the cursor position to start of navbar
            Console.SetCursorPosition(0, _navbarStartLine);

            for (int i = 0; i < _menuItems.Count; i++)
            {
                // Apply background color for selected item
                if (i == _selectedItemIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                else if (_menuItems[i] == _customerView)  // This line has been corrected
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Black; // You might want a background color here too
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                string menuItem = _menuItems[i];
                int padding = (Console.WindowWidth / _menuItems.Count - menuItem.Length) / 2;
                Console.Write(new string(' ', padding) + menuItem + new string(' ', padding));

                Console.ResetColor();
            }

            int intendedPosition = _navbarStartLine + 1;
            if (intendedPosition < Console.BufferHeight)
            {
                Console.SetCursorPosition(0, intendedPosition);
            }

            Console.WriteLine(new string('=', Console.WindowWidth));
        }



        public EnumNavBar? Logic(ConsoleKey key) // Return type is now EnumNavBar?
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _selectedItemIndex = (_selectedItemIndex - 1 + _menuItems.Count) % _menuItems.Count;
                    break;

                case ConsoleKey.RightArrow:
                    _selectedItemIndex = (_selectedItemIndex + 1) % _menuItems.Count;
                    break;

                case ConsoleKey.Enter:
                    if (_menuItems[_selectedItemIndex] == _cartView)
                    {
                        return EnumNavBar.Cart;
                    }
                    else if (_menuItems[_selectedItemIndex] == _customerView)
                    {
                        return EnumNavBar.Welcome;
                    }
                    else if (Enum.TryParse(_menuItems[_selectedItemIndex], out EnumNavBar selectedEnum))
                    {
                        return selectedEnum;
                    }
                    break;
            }
            return null;
        }
        public void Clear()
        {
            for (int i = _navbarStartLine; i <= _navbarEndLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

    }
}