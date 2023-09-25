using System;
using System.Threading;

namespace Lab2.Utils
{
    public enum NotificationType
    {
        Error,
        Success
    }

    public class Notification
    {
        private  int _notificationYPosition;
        private string _message = string.Empty;
      

        public void Show(string highlightedMessage, string message, NotificationType messageType, int durationMilliseconds = 2000, string position = "top")
        {
          switch (position)
            {
            case "center":
                    _notificationYPosition = (Console.BufferHeight/2)-4;
                    break;
                case "bottom":
                    _notificationYPosition = Console.WindowHeight - 2;
                    break;
                case "top":
                    _notificationYPosition = 2;
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;

            // Set background color based on message type
            Console.BackgroundColor = messageType == NotificationType.Error ? ConsoleColor.Red : ConsoleColor.Green;

            // Calculate the starting position to center the combined message
            int totalMessageLength = message.Length + " ".Length + highlightedMessage.Length;
            int notificationPositionX = (Console.WindowWidth - totalMessageLength) / 2;

            string leftPadding = new string(' ', notificationPositionX);
            string rightPadding = new string(' ', Console.WindowWidth - totalMessageLength - leftPadding.Length);

            Console.SetCursorPosition(0, _notificationYPosition);
            Console.Write(leftPadding);

            // Write the highlighted message first
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(highlightedMessage);
            Console.Write(" ");

            // Reset color and write the rest of the message
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);

            Console.Write(rightPadding);

            DateTime endTime = DateTime.Now.AddMilliseconds(durationMilliseconds);
            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true); // Clear the key from the input buffer
                    break; // Exit the loop if a key is pressed
                }
                Thread.Sleep(100); // Check every 100ms
            }



            // Clear the notification
            Console.SetCursorPosition(0, _notificationYPosition);
            Console.ResetColor();
            Console.Write(new string(' ', Console.WindowWidth));
        }

    }
}
