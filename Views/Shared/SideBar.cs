using Lab2.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Views.Shared
{
    internal static class Sidebar
    {   
        private static List<string> Categories;
        private static int SelectedCategoryIndex = 0;
        private static readonly int XPosition = 15;
        private static readonly int SidebarStartLine = 4;
        private static readonly int SidebarEndLine = Console.WindowHeight - 4;

        static Sidebar()
        {
            Categories = ProductService.GetDistinctCategories().ToList();
        }

        public static string Render()
        {
            string selectedCategory;

            do
            {
                ConsoleDrawing.DrawVerticalLine(XPosition, SidebarEndLine);

                
                for (int i = 0; i < Categories.Count; i++)
                {
                    Console.SetCursorPosition(1, SidebarStartLine + 3 * i);

                    if (i == SelectedCategoryIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.Write(Categories[i]);
                    Console.ResetColor();
                }

                
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        SelectedCategoryIndex = (SelectedCategoryIndex - 1 + Categories.Count) % Categories.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        SelectedCategoryIndex = (SelectedCategoryIndex + 1) % Categories.Count;
                        break;
                    case ConsoleKey.Enter:
                        selectedCategory = Categories[SelectedCategoryIndex];
                        return selectedCategory;
                    case ConsoleKey.Escape:
                        return null;
                }

            } while (true); 
        }

        public static class ConsoleDrawing
        {
            public static void DrawVerticalLine(int xPosition, int height)
            {
                for (int i = SidebarStartLine; i < height; i++)
                {
                    Console.SetCursorPosition(xPosition, i);
                    Console.Write("|");
                }
            }
        }
    }
}
