using System;

namespace Lab2.Views.Shared
{
    public static class Body
    {
    

        public static void Clear(int startLine = 2, int endline = 2)
        {
            if(endline == 2)
            {
                endline = Console.WindowHeight - 2;
            }

            for (int i = startLine; i < endline - 1; i++)  // -1 to leave a line space above the footer
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}
