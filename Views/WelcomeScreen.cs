using Lab2.Models;
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
            string message = $"Welcome to {StoreConfig.StoreName}";
            int width = message.Length + 4;
            int height = 3;

            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 3;

            DisplayUtils.DrawBox(left, top, width, height);

            
            Console.SetCursorPosition(left + (width - message.Length) / 2, top + 1);
            Console.Write(message);

            string Line1 = "Use Enter, Esc and arrow keys to navigate the menu.";
            Console.SetCursorPosition((Console.WindowWidth - Line1.Length) / 2, top + height + 1);
            Console.WriteLine($"{Line1}");

            string Line2 = "The store provides 3 tiers of discounts: Bronze, Silver and Gold.";
            Console.SetCursorPosition((Console.WindowWidth - Line2.Length) / 2, top + height + 3);
            Console.WriteLine($"{Line2}");

            string Line3 = "The level of discount depends on the total amount of money spent at the store, it is calculated efter checkout.";
            Console.SetCursorPosition((Console.WindowWidth - Line3.Length) / 2, top + height + 4);
            Console.WriteLine( $"{Line3}");

            string Line4 = "At total of 500kr spent will give you Gold Tier, 250kr for Silver and Broze is the default.";
            Console.SetCursorPosition((Console.WindowWidth - Line4.Length) / 2, top + height + 5);
            Console.WriteLine($"{Line4}");



            string goldText = "Gold: 15%";
            string silverText = "Silver: 10%";
            string bronzeText = "Bronze: 5%";
            string separator = " | ";

            
            int totalLength = goldText.Length + silverText.Length + bronzeText.Length + 2 * separator.Length;

          
            int startLeft = (Console.WindowWidth - totalLength) / 2;
            int separatorLine = top + height + 7;

            
            Console.SetCursorPosition(startLeft, separatorLine);

            // Gold
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(goldText);
            Console.ResetColor();
            Console.Write(separator);

            // Silver
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(silverText);
            Console.ResetColor();
            Console.Write(separator);

            // Bronze
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(bronzeText);
            Console.ResetColor();
                
        }
    }



}
