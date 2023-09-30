using Lab2.Models;
using Lab2.Utils;
using Lab2.Views.Shared;
using Lab2.Services;
using System;
using System.Collections.Generic;
using Azure.AI.OpenAI;
using System.Text;

namespace Lab2.ShopAI
{
    public static class ChatUI
    {
        public static int chatTop;
        public static int chatBottom;
        public static ChatMsgs chatMsgs = new ChatMsgs();
        private static ShopAssistant shopAssistant;
        private static bool chatActive = true;

        static ChatUI()
        {
            chatTop = UiHelper.NavbarEndPositionTop + 2;
            chatBottom = UiHelper.FooterStartPositionTop - 2;
            shopAssistant = new ShopAssistant();
        }
        public static void Render()
        {
            chatActive = true;
            while (chatActive)
            {
                Body.Clear(chatTop, chatBottom, 3, -3);
                // Header
                string header = "Shop Assistant | Beta | Enter your openAi ApiKey in the ShopAssistant File.";
                Console.SetCursorPosition( Console.WindowWidth / 2 - header.Length / 2, 2);
                Console.WriteLine(header);
                Console.WriteLine(new string('=', Console.WindowWidth));
                // Chat Area
                ChatArea();
                // Handle user input
                InputHandler().Wait();
            }
        }

        public static void ChatArea()
        {
            chatTop = UiHelper.NavbarEndPositionTop + 2;
            Body.Clear(chatTop, chatBottom);
            foreach (var msg in chatMsgs.ChatMsgList)
            {
                Console.SetCursorPosition(2, chatTop);
                Console.Write(msg.User.ToString() + ": ");
                Console.WriteLine(msg.Message);
                chatTop += 2;
            }
        }
        public async static Task InputHandler()
        {
            Console.SetCursorPosition(0, Console.WindowHeight-5);
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.SetCursorPosition(2, Console.WindowHeight-4);
            Console.Write("You: ");

            StringBuilder userMessageBuilder = new StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    chatActive = false;
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && userMessageBuilder.Length > 0)
                {
                    userMessageBuilder.Remove(userMessageBuilder.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else
                {
                    userMessageBuilder.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            var userMessage = userMessageBuilder.ToString();
            Body.Clear(Console.WindowHeight-4, Console.WindowHeight-2, 6);
            Console.SetCursorPosition(6, Console.WindowHeight-4);

            if (string.IsNullOrEmpty(userMessage))
            {
                Notification.Show("Error", "Please enter a valid query.", NotificationType.Error, 2000);
            }
            else
            {
                chatMsgs.ChatMsgList.Add(new Msg { User = ChatUser.User, Message = userMessage });
                string assistantResponse = await shopAssistant.CartMethodsAsync(userMessage);
                chatMsgs.ChatMsgList.Add(new Msg { User = ChatUser.Assistant, Message = assistantResponse });
                ChatArea();
            }
        }
    }

    public class ChatMsgs
    {
        public List<Msg> ChatMsgList { get; set; } = new List<Msg>();
    }

    public class Msg
    {
        public int Id { get; set; } = Guid.NewGuid().GetHashCode();
        public ChatUser User { get; set; }
        public string Message { get; set; }
    }

    public enum ChatUser
    {
        User,
        Assistant
    }
}
