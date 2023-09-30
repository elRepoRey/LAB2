using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class UiHelper
    {
        public static int NavbarEndPositionTop { get; set; } = 2;
        public static int NavbarStartPositionTop { get; set; } = 0;
        public static int FooterStartPositionTop { get; set; } = Console.WindowHeight-2;
        public static int GetCenteredLeftPosition(int contentLength)

        {
            return (Console.WindowWidth - contentLength) / 2;
        }
    }

} 