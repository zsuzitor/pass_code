

using PassCode.Models.BL.Interfaces;
using System;

namespace PassCode.Models.BL
{
    public class ConsoleOutput : IOutput
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
