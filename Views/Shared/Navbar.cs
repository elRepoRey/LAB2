
using Lab2.Services;
using Lab2.Utils;
using System;
using System.Collections.Generic;

namespace Lab2.Views.Shared
{
    internal static class Navbar
    {
        private static string? CartView;
        private static List<string>? MenuItems;
        private static int SelectedItemIndex = 0;
        private static decimal CartTotal;     
        private static string? CustomerView;
        private const int MaxCartViewLength = 21;

        private static string? CartTrigger { get; set; }
        public static void FlashCart()
        {
            CartTrigger = CartView;
            UpdateMenuItems();
            System.Threading.Thread.Sleep(150);
            CartTrigger = null;
            UpdateMenuItems();
        }

        public static void UpdateMenuItems()
        {
            CartTotal = CurrencyServices.ConvertToGlobalCurrency(GlobalState.LoggedInCustomer?.Cart.TotalPrice ?? 0);
            CartView = $"{EnumNavBar.Cart} |{GlobalState.LoggedInCustomer?.Cart.Count ?? 0}| |{CurrencyServices.GlobalCurrency} {CartTotal}|";
            CartView = CartView.PadRight(MaxCartViewLength);

            Clear();
            MenuItems = new List<string>
            {
                EnumNavBar.MyCornerStore.ToString()
            };

            if (GlobalState.LoggedInCustomer != null)
            {
                CustomerView = $"{EnumNavBar.Hi} {GlobalState.LoggedInCustomer.Name} |{GlobalState.LoggedInCustomer.Tier}|";
                MenuItems.AddRange(new List<string>
                {
                    EnumNavBar.Settings.ToString(),
                    CartView,
                    CustomerView,
                   // EnumNavBar.ShopAI.ToString(),
                    EnumNavBar.Logout.ToString()
                });
            }

            Render();
        }

        public static void Render()
        {
            if (MenuItems == null) return;

            Console.SetCursorPosition(0, UiHelper.NavbarStartPositionTop);

            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (i == SelectedItemIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                
                else if (MenuItems[i] == CustomerView)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    if (GlobalState.LoggedInCustomer!.Tier == "Silver")
                        Console.ForegroundColor = ConsoleColor.Gray;
                    else if (GlobalState.LoggedInCustomer.Tier == "Gold")
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else if (GlobalState.LoggedInCustomer.Tier == "Bronze")
                        Console.ForegroundColor = ConsoleColor.DarkRed;                
                }
                else if (MenuItems[i] == CartTrigger)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                string menuItem = MenuItems[i];
                int padding = Math.Max((Console.WindowWidth / MenuItems.Count - menuItem.Length) / 2, 0);
                Console.Write(new string(' ', padding) + menuItem + new string(' ', padding));

                Console.ResetColor();
            }

            int intendedPosition = UiHelper.NavbarStartPositionTop + 1;
            if (intendedPosition < Console.BufferHeight)
            {
                Console.SetCursorPosition(0, intendedPosition);
            }

            Console.WriteLine(new string('=', Console.WindowWidth));
        }

        public static EnumNavBar? Logic()
        {
            if (MenuItems == null) return null;

            switch ( Console.ReadKey().Key)
            {
                case ConsoleKey.LeftArrow:
                    SelectedItemIndex = (SelectedItemIndex - 1 + MenuItems.Count) % MenuItems.Count;
                    break;

                case ConsoleKey.RightArrow:
                    SelectedItemIndex = (SelectedItemIndex + 1) % MenuItems.Count;
                    break;

                case ConsoleKey.Enter:
                    if (MenuItems[SelectedItemIndex] == CartView)
                    {
                        return EnumNavBar.Cart;
                    }
                    else if (MenuItems[SelectedItemIndex] == CustomerView)
                    {
                        return EnumNavBar.Hi;
                    }
                    else if (Enum.TryParse(MenuItems[SelectedItemIndex], out EnumNavBar selectedEnum))
                    {
                        return selectedEnum;
                    }
                    break;
            }
            return null;
        }
        public static void Clear()
        {
            for (int i = UiHelper.NavbarStartPositionTop; i <= UiHelper.NavbarEndPositionTop; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
        

    }
}
