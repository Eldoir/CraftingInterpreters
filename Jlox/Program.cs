using Jlox;

public partial class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: jlox [script]");
            return;
        }
        else if (args.Length == 1)
        {
            if (!RunFile(args[0]))
            {
                Console.WriteLine("There was an error executing the file.");
            }
        }
        else
        {
            RunPrompt();
        }
    }

    /// <returns>If there was an error.</returns>
    private static bool RunFile(string path)
    {
        Run(File.ReadAllText(path));
        return _hadError;
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null) break;
            Run(line);
            _hadError = false; // if the user makes a mistake, it shouldn't kill their entire session
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new(source);
        List<Token> tokens = scanner.ScanTokens();

        // For now, just print the tokens.
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, string.Empty, message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        _hadError = true;
    }

    private static bool _hadError = false;
}