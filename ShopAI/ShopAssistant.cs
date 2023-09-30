using Azure;
using Azure.AI.OpenAI;
using Lab2.ShopAI;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Text;
using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;

namespace Lab2.ShopAI;
public class ShopAssistant
{
    private readonly OpenAIClient client;
    private static readonly string Model = "gpt-4-0613";
    private readonly string ApiKey;
    private readonly string AssistantPrompt;
    private List<ChatMessage> conversationMessages;
    private List<Product> products;



    public ShopAssistant()
    {
        products = ProductService.GetAllProducts().ToList();
        ApiKey = "Your OpenAi Api Key";
        client = new OpenAIClient(ApiKey);
        StringBuilder AssistantPromptBuilder = new StringBuilder();
        AssistantPrompt = "You are a shop assistant. You can check the availability of items or add them" +
            " to a user's shopping cart. When adding an item to the cart, ensure you use the correct casing" +
            ". For instance, 'apples' or 'apple' should be written as 'Apple'. Please refer to the product" +
            " list for proper naming conventions: bellow you have the list of product in the store ";
        AssistantPromptBuilder.Append(AssistantPrompt);
        foreach (var product in products)
        {
            AssistantPromptBuilder.Append($"{product.Name}, ");
            
        }
        AssistantPromptBuilder.Append("are available in the store.");
        conversationMessages = new List<ChatMessage>()
        {
              new(ChatRole.System, AssistantPrompt)
        };
    }
    public async Task<string> CartMethodsAsync(string userMessage)
    {
        conversationMessages.Add(new(ChatRole.User, userMessage));

        var CartMethods = new FunctionDefinition
        {
            Name = "CartMethods",
            Description = "Add products to cart.",
            Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        MethodToCall = new
                        {
                            Type = "string",
                            Description = "Output Add , to add an item to the user's " +
                            "shopping cart"
                        },
                        itemName = new
                        {
                            Type = "string",
                            Description = "The name of the item to add to cart"
                        },                        
                    },
                    Required = new[] { "itemName" }
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        };

        var chatCompletionsOptions = new ChatCompletionsOptions();
        foreach (var chatMessage in conversationMessages)
        {
            chatCompletionsOptions.Messages.Add(chatMessage);
        }

        chatCompletionsOptions.Functions.Add(CartMethods);
        try
        {
            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(
                Model,
                chatCompletionsOptions
            );

            var chatCompletionResponses = response.Value.Choices;

            if (chatCompletionResponses != null && chatCompletionResponses.Count > 0)
            {
                var functionCall = chatCompletionResponses[0].Message.FunctionCall;
                var contentMessage = chatCompletionResponses[0].Message.Content;

                if (functionCall != null) 
                {
                    var functionArguments = JsonConvert.DeserializeObject<Dictionary<string, string>>
                        (functionCall.Arguments);

                    string methodToCall = functionArguments["methodToCall"];
                    string itemName = functionArguments["itemName"];

                    if (!string.IsNullOrEmpty(methodToCall) && !string.IsNullOrEmpty(itemName))
                    {                      
                        string gatewayResponse = FunctionGateway.Gateway(methodToCall, itemName);
                        StringBuilder SystemPromptBuilder = new StringBuilder();
                       
                        SystemPromptBuilder.Append("this is the response from the from the store system, " +
                            "confirming if the item is available or not or " +
                            "that the items has be added to the cart. respond to the user with that info: ");
                        SystemPromptBuilder.Append(gatewayResponse);
                        conversationMessages.Add(new(ChatRole.User, gatewayResponse));                        
                        
                        foreach (var chatMessage in conversationMessages)
                        {
                            chatCompletionsOptions.Messages.Add(chatMessage); 
                        }

                        response = await client.GetChatCompletionsAsync(Model, chatCompletionsOptions);                        
                       
                        return conversationMessages[conversationMessages.Count - 1].Content;
                    }
                    else
                    {
                        return "Invalid function call arguments.";
                    }
                }
                else if (!string.IsNullOrEmpty(contentMessage))
                { 
                  
                    return contentMessage;
                }
            }

            return "No valid response from the assistant.";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
} 

public class ChatCompletionResponse
{
    public MessageDetail Message { get; set; }
}

public class MessageDetail
{
    public FunctionCallDetail FunctionCall { get; set; }
}

public class FunctionCallDetail
{
    public string Name { get; set; }
    public string Arguments { get; set; }
}

