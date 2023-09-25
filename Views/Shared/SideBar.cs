using Lab2.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Views.Shared
{
    internal class Sidebar
    {
        private List<string> _categories;
        private int _selectedCategoryIndex = 0;
        private const int XPosition = 15;
        private const int _sidebarStartLine = 4;
        private int _sidebarEndLine = Console.WindowHeight - 4;

        public Sidebar(ProductService productService)
        {
            _categories = productService.GetDistinctCategories().ToList();
        }

        public string ItemSelection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _selectedCategoryIndex = (_selectedCategoryIndex - 1 + _categories.Count) % _categories.Count;
                    break;
                case ConsoleKey.DownArrow:
                    _selectedCategoryIndex = (_selectedCategoryIndex + 1) % _categories.Count;
                    break;
                case ConsoleKey.Enter:
                    return _categories[_selectedCategoryIndex];
                default:
                    return null;
            }
            return null;
        }

        public string Render()
        {
            ConsoleKey key;

            do
            {
                ConsoleDrawing.DrawVerticalLine(XPosition, _sidebarEndLine);

                // Render all categories
                for (int i = 0; i < _categories.Count; i++)
                {
                    Console.SetCursorPosition(1, _sidebarStartLine + 3 * i);

                    if (i == _selectedCategoryIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.Write(_categories[i]);
                    Console.ResetColor();
                }

                key = Console.ReadKey().Key;
              
                string selected = ItemSelection(key);
                
                if (selected != null)
                {
                    
                    return selected;
                } 

            } while (key != ConsoleKey.Escape);

            // Ensure null is returned as default after the loop
            return null;
        }

        public static class ConsoleDrawing
        {
            public static void DrawVerticalLine(int xPosition, int height)
            {
                for (int i = _sidebarStartLine; i < height; i++)
                {
                    Console.SetCursorPosition(xPosition, i);
                    Console.Write("|");
                }
            }
        }
    }
}
