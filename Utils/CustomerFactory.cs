using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Models;

namespace Lab2.Utils
{ 
    internal class CustomerFactory
    {
        internal class GoldCustomer : Customer
        {
            public override decimal DiscountRate => 0.15M;

            public GoldCustomer(string name, string password, decimal accumulatedPurchaseAmount) : 
                base(name, password, accumulatedPurchaseAmount, "Gold")
            {
                
            }
        }

        internal class SilverCustomer : Customer
        {
            public override decimal DiscountRate => 0.10M;

            public SilverCustomer(string name, string password, decimal accumulatedPurchaseAmount) : 
                base(name, password, accumulatedPurchaseAmount,"Silver")
            {
                
            }
        }

        internal class BronzeCustomer : Customer
        {
            public override decimal DiscountRate => 0.05M;

            public BronzeCustomer(string name, string password, decimal accumulatedPurchaseAmount) : 
                base(name, password, accumulatedPurchaseAmount,"Bronze")
            {
                
            }
        }

        

        public static  Customer SetCustomer(string name, string password, decimal accumulatedPurchaseAmount)
        {
            if (accumulatedPurchaseAmount >= 500)
            {
                return new GoldCustomer(name, password, accumulatedPurchaseAmount);
            }
            else if (accumulatedPurchaseAmount >= 250)
            {
                return new SilverCustomer(name, password, accumulatedPurchaseAmount);
            }
           
            return new BronzeCustomer(name, password, accumulatedPurchaseAmount);
        }
    }
}
