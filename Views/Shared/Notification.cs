using System;
using System.Threading;

namespace Lab2.Utils
{
    public enum NotificationType
    {
        Error,
        Success
    }

    public static class Notification
    {
        private static  int NotificationYPosition = 2;
        private static string Message = string.Empty;
      

        public static void Show(string highlightedMessage, string message, NotificationType messageType, int durationMilliseconds = 2000, string position = "top")
        {
          switch (position)
            {
            case "center":
                    NotificationYPosition = (Console.BufferHeight/2)-4;
                    break;
                case "bottom":
                    NotificationYPosition = Console.WindowHeight - 2;
                    break;
                case "top":
                    NotificationYPosition = 2;
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
           
            Console.BackgroundColor = messageType == NotificationType.Error ? ConsoleColor.Red : ConsoleColor.Green;
           
            int totalMessageLength = message.Length + " ".Length + highlightedMessage.Length;
            int notificationPositionX = (Console.WindowWidth - totalMessageLength) / 2;

            string leftPadding = new string(' ', notificationPositionX);
            string rightPadding = new string(' ', Console.WindowWidth - totalMessageLength - leftPadding.Length);

            Console.SetCursorPosition(0, NotificationYPosition);
            Console.Write(leftPadding);

           
            Console.ForegroundColor = NotificationType.Error == messageType ? ConsoleColor.White : ConsoleColor.Black;
            Console.Write(highlightedMessage);
            Console.Write(" ");

           
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(message);

            Console.Write(rightPadding);

            DateTime endTime = DateTime.Now.AddMilliseconds(durationMilliseconds);
            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true); 
                    break;
                }
                Thread.Sleep(50); 
            }
            
            Console.SetCursorPosition(0, NotificationYPosition);
            Console.ResetColor();
            Console.Write(new string(' ', Console.WindowWidth));
        }

    }
}
