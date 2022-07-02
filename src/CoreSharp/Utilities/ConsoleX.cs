using System;
using System.Globalization;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Console"/> utilities.
/// </summary>
public static class ConsoleX
{
    /// <summary>
    /// Clear current line.
    /// </summary>
    public static void ClearLine()
    {
        var line = Console.CursorTop;
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
    public static void WaitForEnter()
        => Console.ReadLine();

    /// <summary>
    /// New line.
    /// </summary>
    public static void NewLine()
        => WriteLine();

    /// <inheritdoc cref="Write(string)"/>
    public static void Write(object input)
        => Write($"{input}");

    /// <inheritdoc cref="Write(string)"/>
    public static void Write(IFormatProvider formatProvider, string format, params object[] parameters)
    {
        var message = string.Format(formatProvider, format, parameters);
        Write(message);
    }

    /// <inheritdoc cref="Write(string)"/>
    public static void Write(string format, params object[] parameters)
    {
        var message = string.Format(format, parameters);
        Write(message);
    }

    /// <summary>
    /// Write data.
    /// </summary>
    public static void Write(string message)
        => Console.Write(message);

    /// <inheritdoc cref="WriteLine(string)"/>
    public static void WriteLine(object input)
        => WriteLine($"{input}");

    /// <inheritdoc cref="WriteLine(string)"/>
    public static void WriteLine(IFormatProvider formatProvider, string format, params object[] parameters)
    {
        var message = string.Format(formatProvider, format, parameters);
        WriteLine(message);
    }

    /// <inheritdoc cref="WriteLine(string)"/>
    public static void WriteLine(string format, params object[] parameters)
    {
        var message = string.Format(format, parameters);
        WriteLine(message);
    }

    /// <summary>
    /// Write line.
    /// </summary>
    public static void WriteLine(string message)
        => Console.WriteLine(message);

    /// <inheritdoc cref="WriteLine(string)"/>
    public static void WriteLine()
        => WriteLine(string.Empty);

    /// <inheritdoc cref="Log(string)"/>
    public static void Log(IFormatProvider formatProvider, string format, params object[] parameters)
    {
        var data = string.Format(formatProvider, format, parameters);
        Log(data);
    }

    /// <inheritdoc cref="Log(string)"/>
    public static void LogCI(string format, params object[] parameters)
        => Log(CultureInfo.InvariantCulture, format, parameters);

    /// <summary>
    /// Log message with timestamp.
    /// </summary>
    public static void Log(string message)
    {
        var timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

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
