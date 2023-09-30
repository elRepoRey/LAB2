using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Views
{
    internal static class AuthFlow
    {
        private static readonly Dictionary<string, int> MainPosition;
        private static readonly int NotificationDuration = 4000;
        

        static AuthFlow()
        {
            MainPosition = new Dictionary<string, int>
            {
                { "left", Console.WindowWidth /2},
                { "top", (Console.WindowHeight  / 3)  }
            };
        }
        private static void UpdateMainPosition()
        {
            MainPosition["left"] = Console.WindowWidth / 2;
            MainPosition["top"] = (Console.WindowHeight / 3);
        }

        public static void Render()
        {
            int currentSelection = 0;
            ConsoleKey key;
            bool shouldExit = false;

            do
            {
                Body.Clear();
                DisplayTitle();
                DisplayOptions(currentSelection);

                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        currentSelection = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        currentSelection = 1;
                        break;
                    case ConsoleKey.Enter:
                        if (currentSelection == 0)
                        {
                            Login();
                            if (GlobalState.LoggedInCustomer != null)
                            {
                                shouldExit = true;
                            }
                        }
                        else if (currentSelection == 1)
                        {
                            Register();
                            if (GlobalState.LoggedInCustomer != null) 
                            {
                                shouldExit = true;
                            }
                        }
                        break;
                }
            } while (!shouldExit); 
        }

        private static void Login()
        {
            GlobalState.LoggedInCustomer = null; 
            var CustomerName = GetInput("Enter your username:", "Name can't be empty", 1);
            if (CustomerName == null) return;

            var password = GetInput("Enter your password:", "Password can't be empty", 4);
            if (password == null) return;

           Enum status = CustomerServices.Login(CustomerName, password);
            switch (status)
            {
                case EnumLoginStatus.Success:                   
                    Body.Clear();
                                     
                    break;
                case EnumLoginStatus.UserDoesNotExist:
                    Notification.Show(CustomerName, "doesn't exist. Try again or register as a new user.", NotificationType.Error, NotificationDuration);
                    break;
                case EnumLoginStatus.IncorrectPassword:
                    Notification.Show("Error", "Password is incorrect. Try again.", NotificationType.Error);
                    break;
                default:
                    Notification.Show("Error", "An unexpected error occurred. Please try again later.", NotificationType.Error);
                    break;
            }
           
        }

        private static void Register()
        {
            GlobalState.LoggedInCustomer = null; 

            var CustomerName = GetInput("Enter desired username:", "Name can't be empty", 1);
            if (CustomerName == null) return;

            var password = GetInput("Enter desired password:", "Password can't be empty", 4);
            if (password == null) return;

            (string? LoggedInCustomerName, Enum status) = CustomerServices.Register(CustomerName, password);
            switch (status)
            {
                case EnumRegisterStatus.Success:                   
                    Body.Clear();
                    Notification.Show($"{LoggedInCustomerName}", $"Welcome! You have successfully registered.", NotificationType.Success, NotificationDuration, "center");                     
                    break;
                case EnumRegisterStatus.UserAlreadyExists:
                    Notification.Show("Error", "Customer with the provided name already exists.", NotificationType.Error);
                    break;
                case EnumRegisterStatus.Error:
                default:
                    Notification.Show("Error", "An error occurred while trying to register. Please try again.", NotificationType.Error);
                    break;
            }
        }


        private static string? GetInput(string prompt, string errorMessage, int yOffset)
        {
            while (true)
            {
                DrawInputBorder(prompt.Length, yOffset);

                Console.SetCursorPosition(MainPosition["left"] - prompt.Length / 2, MainPosition["top"] + yOffset + 1);
                Console.Write(prompt);

                Console.SetCursorPosition(MainPosition["left"] - prompt.Length / 2, MainPosition["top"] + yOffset + 2);
                var input = CustomReadLine();

                if (input == "ESCAPE_KEY_PRESSED")
                {
                    return null; 
                }
                else if (string.IsNullOrEmpty(input))
                {
                    Notification.Show("Error", errorMessage, NotificationType.Error, 2300);
                }
                else
                {
                    return input;
                }
            }
        }

        private static void DrawInputBorder(int promptLength, int yOffset)
        {
            UpdateMainPosition();
            int width = promptLength + 10;
            Console.SetCursorPosition(MainPosition["left"] - width / 2, MainPosition["top"] + yOffset);
            Console.Write("+" + new string('-', width) + "+");

            for (int i = 1; i <= 3; i++) 
            {
                Console.SetCursorPosition(MainPosition["left"] - width / 2, MainPosition["top"] + yOffset + i);
                Console.Write("|" + new string(' ', width) + "|");
            }

            Console.SetCursorPosition(MainPosition["left"] - width / 2, MainPosition["top"] + yOffset + 4);
            Console.Write("+" + new string('-', width) + "+");
        }


        private static string? CustomReadLine()
        {
            var input = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return input.ToString();
                    case ConsoleKey.Escape:
                        return "ESCAPE_KEY_PRESSED"; 
                    case ConsoleKey.Backspace when input.Length > 0:
                        Console.Write("\b \b");
                        input.Remove(input.Length - 1, 1);
                        break;
                    default:
                        if (char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsPunctuation(keyInfo.KeyChar) ||
                            char.IsSymbol(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar))
                        {
                            Console.Write(keyInfo.KeyChar);
                            input.Append(keyInfo.KeyChar);
                        }
                        break;
                }
            }
        }


        private static void DisplayTitle()           
        {
            UpdateMainPosition();
            string title = $"Welcome to the {StoreConfig.StoreName}!";
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition((MainPosition["left"] - $"{title}".Length / 2)+1, MainPosition["top"] -2 );
            Console.WriteLine($"{title}");
            Console.ResetColor();
            Console.SetCursorPosition((MainPosition["left"] - $"{title}".Length / 2)+1, MainPosition["top"] - 1);
            Console.WriteLine(new string('-', $"{title}".Length));       
                    
        }

        private static void DisplayOptions(int currentSelection)
        {
            UpdateMainPosition();
            int buttonWidth = 10; 
            int spacingBetweenButtons = 4; 

            string loginButton = CreateButton("Login", buttonWidth);
            string registerButton = CreateButton("Register", buttonWidth);

            int totalWidth = 2 * buttonWidth + spacingBetweenButtons;

            Console.SetCursorPosition(MainPosition["left"] - totalWidth / 2, MainPosition["top"]);
            Console.Write(currentSelection == 0 ? HighlightOption(loginButton) : loginButton);

            Console.SetCursorPosition(MainPosition["left"] - totalWidth / 2 + buttonWidth + spacingBetweenButtons, MainPosition["top"]);
            Console.Write(currentSelection == 1 ? HighlightOption(registerButton) : registerButton);
        }

        private static string CreateButton(string text, int width)
        {
            int padding = (width - text.Length) / 2;
            return "[" + new string(' ', padding) + text + new string(' ', width - text.Length - padding) + "]";
        }

        private static string HighlightOption(string text)
        {
            return $"\u001b[42m\u001b[30m{text}\u001b[0m";
        }


    }


}

