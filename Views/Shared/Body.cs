using System;

namespace Lab2.Views.Shared
{
    public static class Body
    {
    

        public static void Clear(int startLine = 2, int endline = 2, int leftStart = 0 , int LeftEnd = 0)
        {
            if(endline == 2)
            {
                endline = Console.WindowHeight - 2;
            }

            for (int i = startLine; i < endline - 1; i++) 
            {
                Console.SetCursorPosition(leftStart, i);
                Console.Write(new string(' ', Console.WindowWidth - LeftEnd));
            }
        }
    }
}
