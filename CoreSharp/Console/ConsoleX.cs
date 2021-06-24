using System;
using System.Globalization;

namespace CoreSharp.Console
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
            int line = System.Console.CursorTop;
            ClearLine(line);
        }

        /// <summary>
        /// Clear line. 
        /// </summary>
        public static void ClearLine(int line)
        {
            var emptyLine = new string(' ', System.Console.WindowWidth);
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(emptyLine);
            System.Console.SetCursorPosition(0, line);
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
        public static void WaitForEnter()
        {
            System.Console.ReadLine();
        }

        /// <summary>
        /// New line. 
        /// </summary>
        public static void NewLine()
        {
            WriteLine();
        }

        /// <summary>
        /// Write input. 
        /// </summary> 
        public static void Write(object input)
        {
            var message = $"{input}";
            Write(message);
        }

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
        public static void Write(string message)
        {
            System.Console.Write(message);
        }

        /// <summary>
        /// Write input. 
        /// </summary> 
        public static void WriteLine(object input)
        {
            string message = input.ToString();
            WriteLine(message);
        }

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
        public static void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        /// <summary>
        /// Write new line. 
        /// </summary>
        public static void WriteLine()
        {
            WriteLine(string.Empty);
        }

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
        {
            Log(CultureInfo.InvariantCulture, format, parameters);
        }

        /// <summary>
        /// Log message with timestamp. 
        /// </summary>
        public static void Log(string message)
        {
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

            System.Console.ResetColor();
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Write("[");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write(timestamp);
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Write("]");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write(" ");
            System.Console.WriteLine(message);
        }
    }
}
