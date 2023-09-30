using Lab2.Models;
using Lab2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab2.ShopAI
{
    internal static class FunctionGateway
    {
        public static string Gateway(string functionName, string itemName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return "Function name is null or empty.";
            }

            if (Enum.TryParse(typeof(EnumCartMethods), functionName, true, out var parsedEnum))
            {
                return CartMethods((EnumCartMethods)parsedEnum, itemName);
            }
            else
            {
                return "Invalid Enum Type.";
            }
        }

        private static string CartMethods(EnumCartMethods functionName, string itemName)
        {
            if (functionName == null)
            {
                return "Function is null.";
            }
            if (functionName.Equals(EnumCartMethods.Check))
            {
                if (CheckAvailability(itemName))
                {
                    return $"Product {itemName} is available.";
                }
                else
                {
                    return $"Product {itemName} is not available.";
                }
              
            }

            if (functionName.Equals(EnumCartMethods.Add))
            {
                if (CheckAvailability(itemName))
                {
                    Product? product = BotDatabaseAccess.StoreProducts
                                    .FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                    GlobalState.LoggedInCustomer!.Cart.Add(product);
                    return $"Product {itemName} added to cart.";
                }
                else
                {
                    return $"Product {itemName} is not available.";
                }
            }
            else
            {
                return "Invalid function enum value.";
            }
        }

        private static bool CheckAvailability(string itemName)
        {
            Product? product = BotDatabaseAccess.StoreProducts
                                .FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            return product != null;
        }
    }
}
