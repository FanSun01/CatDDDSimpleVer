using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatSimpleVer.Common
{
    public static class ConsoleHelper
    {
        public static readonly object _obj = new object();
        public static void WriteColorLine(string str, ConsoleColor color)
        {
            lock (_obj)
            {
                ConsoleColor lastColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(str);
                Console.ForegroundColor = lastColor;
            }
        }
    }
}
