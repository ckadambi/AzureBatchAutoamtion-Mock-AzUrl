namespace AzureJobAutomation.Utils;

public sealed class SimpleLogger
{
    private static readonly object _lock = new();

    private static void Write(string tag, ConsoleColor color, string message)
    {
        lock (_lock)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write($"[{tag}] ");
            Console.ForegroundColor = prev;
            Console.WriteLine(message);
        }
    }

    public void Info(string msg) => Write("INFO", ConsoleColor.Gray, msg);
    public void Warn(string msg) => Write("WARN", ConsoleColor.Yellow, msg);
    public void Error(string msg) => Write("ERROR", ConsoleColor.Red, msg);
    public void Success(string msg) => Write("OK", ConsoleColor.Green, msg);
}
