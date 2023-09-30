using Lab2.Models;
using Lab2.Utils;
using Lab2.Views.Shared;
using Lab2.Services;
using Lab2.ShopAI;

namespace Lab2.Views
{
    internal class Layout
    {
        List<string> products = new()
        {
            "Apple"
        };
            
         public void Render()
        {
            do
            {
                Console.Clear();
                if (GlobalState.LoggedInCustomer == null)
                {
                    Footer.Render();                  
                    AuthFlow.Render();
                }
                else
                {
                    
                    Navbar.UpdateMenuItems();
                    Footer.Render();
                    WelcomeMessage.Render();

                    HandleUserInput();
                }
               
            }
            while (true);
        }

        private void HandleUserInput()
        {
            EnumNavBar? selectedAction = Navbar.Logic();

            switch (selectedAction)
            {
                case EnumNavBar.MyCornerStore:
                    Body.Clear();
                    Store.Render();
                    break;
                case EnumNavBar.Cart:
                    Body.Clear();
                    CheckoutCart.Render();
                    break;
                case EnumNavBar.Settings:
                    Body.Clear();
                    Settings.Render();
                    break;
                case EnumNavBar.Logout:
                    HandleLogout();
                    break;
                case EnumNavBar.Hi:
                    CustomerView.Render();
                    break;
               /* case EnumNavBar.ShopAI:
                    Body.Clear();
                    ChatUI.Render();
                    break;*/
            }
        }      

        private void HandleLogout()
        {
            GlobalState.Logout();
            Render();
        }
    }
}
