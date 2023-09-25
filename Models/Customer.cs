using Lab2.Interfaces;
using Lab2.Services;
using Lab2.Utils;
using System.Text;

namespace Lab2.Models
{
   internal abstract class Customer
    {
        public string Name { get; private set; }
        private string Password { get; set; }
        public Cart Cart { get; set; } = new Cart();
        public string Tier { get;private set; }
        private decimal AccumulatedPurchaseAmount { get ; set; }
        public abstract decimal DiscountRate { get;}
        
        public Customer(string name, string password, decimal accumulatedPurchaseAmount, string Tier = "Bronze")
        {
            Name = name;
            Password = password;
            AccumulatedPurchaseAmount = accumulatedPurchaseAmount;
            this.Tier = Tier;
        }

        
        public void CustomerProfile()
        {
            Console.WriteLine($"Name: {Name}\nPassword: {Password}\nTier:" +
                $" {Tier}\nAccumulated Purchase Amount: {AccumulatedPurchaseAmount}");
        }

        public override string ToString()
        {
            string ConvertedAccumulatedPurchaseAmount = $"{GlobalState.CurrencyManager.ConvertToGlobalCurrency(AccumulatedPurchaseAmount)} {GlobalState.CurrencyManager.GlobalCurrency}";
            StringBuilder sb = new StringBuilder();

            string title = "===== Customer Details =====";
            sb.AppendLine(title.PadLeft(title.Length + (Console.WindowWidth - title.Length) / 2));

            sb.AppendLine($"Name: {Name}".PadLeft((Console.WindowWidth + $"Name: {Name}".Length) / 2));
            sb.AppendLine($"Password: {Password}".PadLeft((Console.WindowWidth + $"Password: {Password}".Length) / 2));
            sb.AppendLine($"Tier: {Tier}".PadLeft((Console.WindowWidth + $"Tier: {Tier}".Length) / 2));
            sb.AppendLine($"Accumulated Purchase Amount: {ConvertedAccumulatedPurchaseAmount}".PadLeft((Console.WindowWidth + $"Accumulated Purchase Amount: {ConvertedAccumulatedPurchaseAmount}".Length) / 2));
            sb.AppendLine();

            string cartTitle = "===== Cart Items =====";
            sb.AppendLine(cartTitle.PadLeft(cartTitle.Length + (Console.WindowWidth - cartTitle.Length) / 2));

            foreach (var cartItem in Cart.CartItems)
            {
                string item = $"{cartItem.Product} x {cartItem.Quantity}";
                sb.AppendLine(item.PadLeft((Console.WindowWidth + item.Length) / 2));
            }

            sb.AppendLine();
            if (Cart.CartItems.Count == 0)
            {
                string empty = "Cart is empty";
                sb.AppendLine(empty.PadLeft((Console.WindowWidth + empty.Length) / 2));
            }
            else
            {
                string totalPrice = $"Total Price: {GlobalState.CurrencyManager.GlobalCurrency} {GlobalState.CurrencyManager.ConvertToGlobalCurrency(Cart.TotalPrice)}";
                sb.AppendLine(totalPrice.PadLeft((Console.WindowWidth + totalPrice.Length) / 2));
            }     

            return sb.ToString();
        }


    }
}
