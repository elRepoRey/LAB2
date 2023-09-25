using Lab2.Models;
using Lab2.Utils;
using Lab2.Views.Shared;
using Lab2.Services;

namespace Lab2.Views
{
    internal class Layout
    {
        private readonly Navbar _navbar;

        private readonly Footer _footer;
        private readonly AuthFlow _authFlow;
        private readonly Settings _settings;
        private readonly Store _store;
        private readonly CheckoutCart _checkoutCart;



        public Layout(Navbar navbar, Footer footer, AuthFlow authFlow, Settings settings, Store store, CheckoutCart checkoutCart)
        {
            _navbar = navbar;
            _footer = footer;
            _authFlow = authFlow;
            _settings = settings;
            _store = store;
            _checkoutCart = checkoutCart;


        }

        public void Render()
        {
            do
            {
                Console.Clear();
                if (GlobalState.LoggedInCustomer == null)
                {

                    _footer.Render();
                    _navbar.UpdateMenuItems();
                    _authFlow.Render();


                }
                _navbar.UpdateMenuItems();
                _footer.Render();
                DisplayWelcomeMessage();
                HandleUserInput(Console.ReadKey().Key);
            }
            while (true);

        }

        private void HandleUserInput(ConsoleKey key)
        {
            EnumNavBar? selectedAction = _navbar.Logic(key);


            switch (selectedAction)
            {
                case EnumNavBar.MyCornerStore:

                    DisplayStore();
                    break;
                case EnumNavBar.Cart:
                    DisplayCart();
                    break;
                case EnumNavBar.Settings:
                    DisplaySettings();
                    break;
                case EnumNavBar.Logout:
                    HandleLogout();
                    break;
                case EnumNavBar.Welcome:
                    DisplayCustomerView();
                    break;
            }
        }

        private void DisplayStore()
        {
            Body.Clear();
            _store.Render();
        }

        private void DisplayCart()
        {
            Body.Clear();
            _checkoutCart.Render();
        }

        private void DisplaySettings()
        {
            Body.Clear();
            _settings.DisplayAndChangeCurrency();
        }

        private void DisplayCustomerView()
        {
            CustomerView.Render();

        }

        private void DisplayWelcomeMessage()
        {
           WelcomeMessage.Render();

        }
        private void HandleLogout()
        {
            GlobalState.Logout();
            Render();
        }

       
    }
}
