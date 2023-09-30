using System;
using System.Collections.Generic;
using System.Linq;

using Lab2.Models;
using Lab2.Utils;

namespace Lab2.Services
{
    internal static class CustomerServices
    {
        
        private static List<CustomerDAO> Customers;
        

        static CustomerServices()
        {
            Customers = DatabaseService<CustomerDAO>.LoadDataFromFiles(StoreConfig.FilePaths["Customers"]);
        }

        public static (string? LoggedInCustomerName, EnumRegisterStatus) Register(string Name, string Password)
        {
            if (Customers.Any(c => c.Name == Name))
            {
                return (null, EnumRegisterStatus.UserAlreadyExists);
            }

            CustomerDAO customerDao = new CustomerDAO(Name, Password);
            Customers.Add(customerDao);
            try
            {
                SaveData();
            }
            catch (DataAccessException)
            {
                return (null, EnumRegisterStatus.Error);
            }

            GlobalState.LoggedInCustomer = CustomerFactory.SetCustomer(customerDao.Name, customerDao.Password, customerDao.AccumulatedPurchaseAmount);
            return (GlobalState.LoggedInCustomer.Name, EnumRegisterStatus.Success);
        }

        public static EnumLoginStatus  Login(string name, string password)
        {
            CustomerDAO? customerDao = Customers.FirstOrDefault(c => c.Name == name);

            if (customerDao == null)
            {
                return EnumLoginStatus.UserDoesNotExist;
            }
            else if (!customerDao.VerifyPassword(password))
            {
                return EnumLoginStatus.IncorrectPassword;
            }

            GlobalState.LoggedInCustomer = CustomerFactory.SetCustomer(customerDao.Name, customerDao.Password, customerDao.AccumulatedPurchaseAmount);
            return EnumLoginStatus.Success;
        }

     
        public static bool Checkout()
        {
            string customerName = GlobalState.LoggedInCustomer?.Name ?? throw new Exception("No customer is logged in.");

            CustomerDAO? customerDao = Customers.FirstOrDefault(c => c.Name == customerName);

            if (customerDao == null)
            {
                throw new CustomerNotFoundException($"Customer with the name {customerName} was not found.");
            }
            
            customerDao.AccumulatedPurchaseAmount += CurrencyServices.ConvertToDatabaseCurrency(GlobalState.LoggedInCustomer.Cart.TotalPrice);
            GlobalState.LoggedInCustomer = CustomerFactory.SetCustomer(customerDao.Name, customerDao.Password, customerDao.AccumulatedPurchaseAmount);
            try
            {
                SaveData();
                GlobalState.LoggedInCustomer.Cart.Clear();
            }
            catch (DataAccessException)
            {
                return false;
            }
            return true;
        }

        private static void SaveData()
        {
            DatabaseService<CustomerDAO>.SaveDataToFiles(Customers, StoreConfig.FilePaths["Customers"]);
        }

        public class CustomerNotFoundException : Exception
        {
            public CustomerNotFoundException(string message) : base(message) { }
        }
    }
}
