using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.AI.OpenAI;

namespace Lab2.ShopAI
{
    internal  class FunctionDefinitions
    {
        
            public FunctionDefinition checkIAvailability = new FunctionDefinition
            {
                Name = "checkAvailability",
                Description = "Check the availability of an item.",
                Parameters = BinaryData.FromObjectAsJson(
                   new
                   {
                       Type = "object",
                       Properties = new
                       {
                           itemName = new
                           {
                               Type = "string",
                               Description = "The name of the item to check."
                           }
                       },
                       Required = new[] { "itemName" }
                   },
                   new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            public FunctionDefinition addToCart = new FunctionDefinition
            {
                Name = "addToCart",
                Description = "Add an item to the user's shopping cart.",
                Parameters = BinaryData.FromObjectAsJson(
                                       new
                                       {
                        Type = "object",
                        Properties = new
                        {
                            itemName = new
                            {
                                Type = "string",
                                Description = "The name of the item to add."
                            }
                        },
                        Required = new[] { "itemName" }
                    },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };
        
    }
}
