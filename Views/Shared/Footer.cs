using Lab2.Models;
using Lab2.Utils;

namespace Lab2.Views.Shared
{
    internal static class Footer
    {
        private static readonly string GitHub;
        private static readonly string LinkedIn;
        private static readonly string FooterText;


        static Footer()
        {
            GitHub = StoreConfig.GitHubUrl;
            LinkedIn = StoreConfig.LinkedInUrl;

            FooterText = $" {LinkedIn}  |  {GitHub}";
        }

        public static void Render()
        {
            Console.SetCursorPosition(0, Console.WindowHeight-2);
            Console.WriteLine(new string('=', Console.WindowWidth));

            int padding = (Console.WindowWidth - FooterText.Length) / 2;
            Console.SetCursorPosition(padding, Console.WindowHeight - 1);
            
            Console.Write(FooterText);
            
        }

    }
}
