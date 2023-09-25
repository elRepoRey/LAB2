using Lab2.Models;
using Lab2.Services;
using Lab2.Utils;
using Lab2.Views.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Views
{
    internal class AuthFlow
    {
        private readonly CustomerServices _customerServices;
        private readonly Notification _notification;
     
        private readonly Dictionary<string, int> _mainPosition;
        

        public AuthFlow(CustomerServices customerServices, Notification notification)
        {
            _customerServices = customerServices;
            _notification = notification;
           

            _mainPosition = new Dictionary<string, int>
            {
                { "left", Console.WindowWidth /2},
                { "top", (Console.WindowHeight  / 2) - (Console.WindowHeight / 3) }
            };
        }

        public void Render()
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
                            if (GlobalState.LoggedInCustomer != null) // If a customer was successfully logged in
                            {
                                shouldExit = true;
                            }
                        }
                        else if (currentSelection == 1)
                        {
                            Register();
                            if (GlobalState.LoggedInCustomer != null) // If a customer was successfully registered
                            {
                                shouldExit = true;
                            }
                        }
                        break;
                }
            } while (!shouldExit); // Exit the loop if shouldExit is true or if ESC is pressed
        }

        private void Login()
        {
            GlobalState.LoggedInCustomer = null;
            var CustomerName = GetInput("Enter your username:", "Name can't be empty", 1);
            if (CustomerName == null) return;

            var password = GetInput("Enter your password:", "Password can't be empty", 4);
            if (password == null) return;

            var (LoggedInCustomerName, status) = _customerServices.Login(CustomerName, password);
            switch (status)
            {
                case EnumLoginStatus.Success:                   
                    Body.Clear();
                                     
                    break;
                case EnumLoginStatus.UserDoesNotExist:
                    _notification.Show(CustomerName, "doesn't exist. Try again or register as a new user.", NotificationType.Error, 2000);
                    break;
                case EnumLoginStatus.IncorrectPassword:
                    _notification.Show("Error", "Password is incorrect. Try again.", NotificationType.Error);
                    break;
                default:
                    _notification.Show("Error", "An unexpected error occurred. Please try again later.", NotificationType.Error);
                    break;
            }
           
        }

        private void Register()
        {
            GlobalState.LoggedInCustomer = null; 


            var CustomerName = GetInput("Enter desired username:", "Name can't be empty", 1, 8);

            if (CustomerName == null) return;

            var password = GetInput("Enter desired password:", "Password can't be empty", 4);
            if (password == null) return;

            var (LoggedInCustomerName, status) = _customerServices.Register(CustomerName, password);
            switch (status)
            {
                case EnumRegisterStatus.Success:                   
                    Body.Clear();
                    _notification.Show($"{LoggedInCustomerName}", $"Welcome! You have successfully registered.", NotificationType.Success, 2000, "center");                     
                    break;
                case EnumRegisterStatus.UserAlreadyExists:
                    _notification.Show("Error", "Customer with the provided name already exists.", NotificationType.Error);
                    break;
                case EnumRegisterStatus.Error:
                default:
                    _notification.Show("Error", "An error occurred while trying to register. Please try again.", NotificationType.Error);
                    break;
            }
        }


        private string? GetInput(string prompt, string errorMessage, int yOffset, int charLimit = int.MaxValue)
        {
            while (true)
            {
                DrawInputBorder(prompt.Length, yOffset);

                Console.SetCursorPosition(_mainPosition["left"] - prompt.Length / 2, _mainPosition["top"] + yOffset + 1);
                Console.Write(prompt);

                Console.SetCursorPosition(_mainPosition["left"] - prompt.Length / 2, _mainPosition["top"] + yOffset + 2);
                var input = CustomReadLine();

                if (input == "ESCAPE_KEY_PRESSED")
                {
                    return null; // This will indicate that the escape key was pressed
                }
                else if (string.IsNullOrEmpty(input))
                {
                    _notification.Show("Error", errorMessage, NotificationType.Error, 2300);
                }
                else if (input.Length > charLimit)
                {
                    _notification.Show("Error", $"Input cannot exceed {charLimit} characters.", NotificationType.Error, 2300);
                }
                else
                {
                    return input;
                }
            }
        }



        private void DrawInputBorder(int promptLength, int yOffset)
        {
            int width = promptLength + 10;
            Console.SetCursorPosition(_mainPosition["left"] - width / 2, _mainPosition["top"] + yOffset);
            Console.Write("+" + new string('-', width) + "+");

            for (int i = 1; i <= 3; i++) // Increased to 3 for padding
            {
                Console.SetCursorPosition(_mainPosition["left"] - width / 2, _mainPosition["top"] + yOffset + i);
                Console.Write("|" + new string(' ', width) + "|");
            }

            Console.SetCursorPosition(_mainPosition["left"] - width / 2, _mainPosition["top"] + yOffset + 4);
            Console.Write("+" + new string('-', width) + "+");
        }


        private string? CustomReadLine()
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
                        return "ESCAPE_KEY_PRESSED"; // Special value to indicate escape key was pressed
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


        private void DisplayTitle()
        {   string title = $"Welcome to the {GlobalState.StoreName}!";
            Console.SetCursorPosition(_mainPosition["left"] - $"{title}".Length / 2, _mainPosition["top"] - 2);
            Console.WriteLine($"{title}");
            Console.SetCursorPosition(_mainPosition["left"] - $"{title}".Length / 2, _mainPosition["top"] - 1);
            Console.WriteLine(new string('-', $"{title}".Length));
            
        }

        private void DisplayOptions(int currentSelection)
        {
            int buttonWidth = 10; 
            int spacingBetweenButtons = 4; // Spaces between the two buttons

            string loginButton = CreateButton("Login", buttonWidth);
            string registerButton = CreateButton("Register", buttonWidth);

            int totalWidth = 2 * buttonWidth + spacingBetweenButtons;

            Console.SetCursorPosition(_mainPosition["left"] - totalWidth / 2, _mainPosition["top"]);
            Console.Write(currentSelection == 0 ? HighlightOption(loginButton) : loginButton);

            Console.SetCursorPosition(_mainPosition["left"] - totalWidth / 2 + buttonWidth + spacingBetweenButtons, _mainPosition["top"]);
            Console.Write(currentSelection == 1 ? HighlightOption(registerButton) : registerButton);
        }

        private string CreateButton(string text, int width)
        {
            int padding = (width - text.Length) / 2;
            return "[" + new string(' ', padding) + text + new string(' ', width - text.Length - padding) + "]";
        }

        private string HighlightOption(string text)
        {
            return $"\u001b[42m\u001b[30m{text}\u001b[0m"; // Green background, black text
        }


    }


}

