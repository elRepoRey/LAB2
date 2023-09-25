using System;
using System.Collections.Generic;
using System.Linq;
using Lab2.Interfaces;
using Lab2.Models;
using Lab2.Utils;

namespace Lab2.Services
{
    internal class CustomerServices
    {
        private readonly DatabaseService<CustomerDAO> _customerDbService;
        private List<CustomerDAO> _items;
        private readonly ICurrencyServices _currencyManager;

        public CustomerServices(DatabaseService<CustomerDAO> customerDbService, ICurrencyServices currencyManager)
        {
            _customerDbService = customerDbService;
            _items = _customerDbService.LoadDataFromFiles();
            _currencyManager = currencyManager;
        }

        public (string LoggedInCustomerName, EnumRegisterStatus status) Register(string Name, string Password)
        {
            if (_items.Any(c => c.Name == Name))
            {
                return (null, EnumRegisterStatus.UserAlreadyExists);
            }

            CustomerDAO customerDao = new CustomerDAO(Name, Password);
            _items.Add(customerDao);
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

        public (string LoggedInCustomerName, EnumLoginStatus status) Login(string name, string password)
        {
            CustomerDAO? customerDao = _items.FirstOrDefault(c => c.Name == name);

            if (customerDao == null)
            {
                return (null, EnumLoginStatus.UserDoesNotExist);
            }
            else if (!customerDao.VerifyPassword(password))
            {
                return (null, EnumLoginStatus.IncorrectPassword);
            }

            GlobalState.LoggedInCustomer = CustomerFactory.SetCustomer(customerDao.Name, customerDao.Password, customerDao.AccumulatedPurchaseAmount);
            return (GlobalState.LoggedInCustomer.Name, EnumLoginStatus.Success);
        }

     
        public bool Checkout()
        {
            string customerName = GlobalState.LoggedInCustomer?.Name ?? throw new Exception("No customer is logged in.");

            CustomerDAO? customerDao = _items.FirstOrDefault(c => c.Name == customerName);

            if (customerDao == null)
            {
                throw new CustomerNotFoundException($"Customer with the name {customerName} was not found.");
            }
            
            customerDao.AccumulatedPurchaseAmount += _currencyManager.ConvertToDatabaseCurrency(GlobalState.LoggedInCustomer.Cart.TotalPrice);
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

        private void SaveData()
        {
            _customerDbService.SaveDataToFiles(_items);
        }

        public class CustomerNotFoundException : Exception
        {
            public CustomerNotFoundException(string message) : base(message) { }
        }
    }
}
