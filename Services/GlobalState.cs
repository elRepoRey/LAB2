using Lab2.Interfaces;
using Lab2.Models;

namespace Lab2.Services
{
    internal static class GlobalState
    {
        private static Customer _loggedInCustomer = null;
        public static CurrencyServices CurrencyManager { get; set; }

        public static string StoreName { get; set; } 

       
        public static void Initialize(CurrencyServices currencyManager, IStoreConfig storeConfig)
        {
            CurrencyManager = currencyManager;
            StoreName = storeConfig.StoreName;
        }

        
        public static event Action<Customer> OnLoginChanged;

        public static Customer LoggedInCustomer
        {
            get => _loggedInCustomer;
            set
            {
                _loggedInCustomer = value;
                OnLoginChanged?.Invoke(_loggedInCustomer);
            }
        }

        public static void Logout()
        {
            _loggedInCustomer = null;
            OnLoginChanged?.Invoke(null);
        }
    }
}
