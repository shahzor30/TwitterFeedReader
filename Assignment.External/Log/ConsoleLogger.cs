using System;

namespace Assignment.External
{
    class ConsoleLogger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine($"Console Logger :=> {msg}");
        }
    }
}