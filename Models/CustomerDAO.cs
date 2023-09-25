using Lab2.Models;

namespace Lab2.Models
{
    internal class CustomerDAO
    {
        public string Name { get; private set; }

        public  string Password { get; private set; }

        public decimal AccumulatedPurchaseAmount { get; set; }

        public CustomerDAO(string name, string password)
        {
            Name = name;
            Password = password;
            AccumulatedPurchaseAmount = 0m;
        }

        public bool VerifyPassword(string password)
        {
            return Password == password;
        }
    }
}
