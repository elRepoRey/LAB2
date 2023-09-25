using Lab2;
using Lab2.Interfaces;

namespace Lab2.Views.Shared
{
    internal class Footer
    {
        private readonly string _gitHub;
        private readonly string _linkedIn;
        private readonly string _footerText;
        

        public Footer(IStoreConfig storeConfig)
        {
            _gitHub = storeConfig.GitHubUrl;
            _linkedIn = storeConfig.LinkedInUrl;
            _footerText = $"Developed by: {_linkedIn} | {_gitHub}";
        }

        public void Render()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.WriteLine(new string('=', Console.WindowWidth));

            int padding = (Console.WindowWidth - _footerText.Length) / 2;
            Console.SetCursorPosition(padding, Console.WindowHeight - 1);
            Console.Write(_footerText);
        }
    }
}
