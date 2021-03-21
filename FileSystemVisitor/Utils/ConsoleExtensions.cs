using System;

namespace FileSystemVisitor.Utils
{
    public static class ConsoleExtensions
    {
        private static ConsoleColor InfoColor = ConsoleColor.Cyan;
        private static ConsoleColor SummaryColor = ConsoleColor.Yellow;
        private static ConsoleColor SuccessColor = ConsoleColor.Green;
        private static ConsoleColor FailColor = ConsoleColor.Red;

        private static ConsoleColor DefaultColor = ConsoleColor.White;

        public static void WriteLineLog(string info)
        {
            Console.ForegroundColor = InfoColor;
            Console.WriteLine(info);
            Console.ForegroundColor = DefaultColor;
        }

        public static void WriteLineSummary(string info)
        {
            Console.ForegroundColor = SummaryColor;
            Console.WriteLine(info);
            Console.ForegroundColor = DefaultColor;
        }

        public static void WriteLineSuccess(string info)
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine(info);
            Console.ForegroundColor = DefaultColor;
        }

        public static void WriteLineFail(string info)
        {
            Console.ForegroundColor = FailColor;
            Console.WriteLine(info);
            Console.ForegroundColor = DefaultColor;
        }
    }
}
