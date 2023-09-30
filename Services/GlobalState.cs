
using Lab2.Models;
using Lab2.Views.Shared;

namespace Lab2.Services
{
    internal static class GlobalState
    {
        private static Customer? _loggedInCustomer;           

        public static Customer? LoggedInCustomer
        {
            get => _loggedInCustomer;
            set
            {
                _loggedInCustomer = value;
                Navbar.UpdateMenuItems();
            }
        }

        public static void Logout()
        {
            _loggedInCustomer = null;
            Navbar.UpdateMenuItems();
        }
    }
}
