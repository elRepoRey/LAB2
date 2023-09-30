using Lab2.Services;
using Lab2.Views.Shared;

namespace Lab2.Views
{
    internal static class CustomerView
    {
        public static void Render()
        {

            do
            {
                if (GlobalState.LoggedInCustomer != null)
                {
                    Body.Clear();
                    Console.SetCursorPosition(0, 4);
                    Console.WriteLine(GlobalState.LoggedInCustomer.ToString());
                    Console.ReadKey();
                    return;
                }

            } while (true);

        }
    }
}
