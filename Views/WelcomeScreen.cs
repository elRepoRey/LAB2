using Lab2.Services;
using System;

namespace Lab2.Views
{
    public static class DisplayUtils
    {
        public static void DrawBox(int left, int top, int width, int height)
        {
            Console.SetCursorPosition(left, top);
            Console.Write("╔" + new string('═', width - 2) + "╗");

            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("║" + new string(' ', width - 2) + "║");
            }

            Console.SetCursorPosition(left, top + height - 1);
            Console.Write("╚" + new string('═', width - 2) + "╝");
        }
    }

    public static class WelcomeMessage
    {
        public static void Render()
        {
            string message = $"Welcome to {GlobalState.StoreName}";
            int width = message.Length + 4;
            int height = 3;

            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 3;

            DisplayUtils.DrawBox(left, top, width, height);

            
            Console.SetCursorPosition(left + (width - message.Length) / 2, top + 1);
            Console.Write(message);

            
            Console.SetCursorPosition((Console.WindowWidth - "Use Enter, Esc and arrow keys to navigate the menu.".Length) / 2, top + height + 1);
            Console.WriteLine("Use Enter, Esc and arrow keys to navigate the menu.");

            Console.SetCursorPosition((Console.WindowWidth - "The store provides 3 tiers of discounts: Bronze, Silver and Gold.".Length) / 2, top + height + 3);
            Console.WriteLine("The store provides 3 tiers of discounts: Bronze, Silver and Gold.");

            Console.SetCursorPosition((Console.WindowWidth - "The level of discount depends on the total amount of money spent at the store, it is calculated efter checkout.".Length) / 2, top + height + 4);
            Console.WriteLine("The level of discount depends on the total amount of money spent at the store, it is calculated efter checkout.");
        }
    }



}
