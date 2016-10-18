namespace AppliedSystems.Infrastucture
{
    using System;

    public static class GreenLogger
    {
        public static void Log(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message, args);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}