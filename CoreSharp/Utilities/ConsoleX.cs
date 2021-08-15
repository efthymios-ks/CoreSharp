using System;
using System.Globalization;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// Extended console functions. 
    /// </summary>
    public static class ConsoleX
    {
        /// <summary>
        /// Clear current line. 
        /// </summary>
        public static void ClearLine()
        {
            int line = Console.CursorTop;
            ClearLine(line);
        }

        /// <summary>
        /// Clear line. 
        /// </summary>
        public static void ClearLine(int line)
        {
            var emptyLine = new string(' ', Console.WindowWidth);
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(emptyLine);
            Console.SetCursorPosition(0, line);
        }

        /// <summary>
        /// Clear list of lines. 
        /// </summary>
        public static void ClearLines(params int[] lines)
        {
            _ = lines ?? throw new ArgumentNullException(nameof(lines));

            foreach (var line in lines)
                ClearLine(line);
        }

        /// <summary>
        /// Wait for enter. 
        /// </summary>
        public static void WaitForEnter() => Console.ReadLine();

        /// <summary>
        /// New line. 
        /// </summary>
        public static void NewLine() => WriteLine();

        /// <summary>
        /// Write input. 
        /// </summary> 
        public static void Write(object input) => Write($"{input}");

        /// <summary>
        /// Write with String.Format. 
        /// </summary>
        public static void Write(IFormatProvider formatProvider, string format, params object[] parameters)
        {
            var message = string.Format(formatProvider, format, parameters);
            Write(message);
        }

        /// <summary>
        /// Write with String.Format. 
        /// </summary>
        public static void Write(string format, params object[] parameters)
        {
            var message = string.Format(format, parameters);
            Write(message);
        }

        /// <summary>
        /// Write with String.Format. 
        /// </summary>
        public static void Write(string message) => Console.Write(message);

        /// <summary>
        /// Write input. 
        /// </summary> 
        public static void WriteLine(object input) => WriteLine($"{input}");

        /// <summary>
        /// Write line with String.Format. 
        /// </summary>
        public static void WriteLine(IFormatProvider formatProvider, string format, params object[] parameters)
        {
            var message = string.Format(formatProvider, format, parameters);
            WriteLine(message);
        }

        /// <summary>
        /// Write line with String.Format. 
        /// </summary>
        public static void WriteLine(string format, params object[] parameters)
        {
            var message = string.Format(format, parameters);
            WriteLine(message);
        }

        /// <summary>
        /// Write line with String.Format. 
        /// </summary>
        public static void WriteLine(string message) => Console.WriteLine(message);

        /// <summary>
        /// Write new line. 
        /// </summary>
        public static void WriteLine() => WriteLine(string.Empty);

        /// <summary>
        /// Log formatted message with timestamp. 
        /// </summary>
        public static void Log(IFormatProvider formatProvider, string format, params object[] parameters)
        {
            string data = string.Format(formatProvider, format, parameters);
            Log(data);
        }

        /// <summary>
        /// Log CultureInvariant formatted message with timestamp. 
        /// </summary>
        public static void LogCI(string format, params object[] parameters)
            => Log(CultureInfo.InvariantCulture, format, parameters);

        /// <summary>
        /// Log message with timestamp. 
        /// </summary>
        public static void Log(string message)
        {
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Write("[");
            Console.ForegroundColor = ConsoleColor.White;
            Write(timestamp);
            Console.ForegroundColor = ConsoleColor.Gray;
            Write("]");
            Console.ForegroundColor = ConsoleColor.White;
            Write(" ");
            WriteLine(message);
        }
    }
}
