using System.IO;

namespace ADL.AdventOfCode2023;

public class Program
{
    private static void Main(string[] args)
    {
        if (args.Length <= 1 || args[1] == "--help" || args[1] == "-h")
        {
            PrintUsageStringAndExit();
        }

        var argIndices = args
        .Select((a, i) => (arg: a, index: i))
        .Where(tuple => tuple.arg.StartsWith("--"));

        int day = -1;
        int part = -1;

        foreach(var (argName, index) in argIndices) {
            var argIndex = index + 1;
            if (argIndex >= args.Length) {
                PrintUsageStringAndExit();
            }
            var argValue = args[argIndex];
            switch(argName) {
                case "--day":
                    if (!int.TryParse(argValue, out day))
                    {
                        PrintUsageStringAndExit();
                        return;
                    }
                    break;
                case "--part":
                    if (!int.TryParse(argValue, out part))
                    {
                        PrintUsageStringAndExit();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }

        var missingParameter = false;
        if (day == -1) {
            Console.WriteLine("Error: missing value for required parameter --day");
            missingParameter = true;
        }
        if (part == -1) {
            Console.WriteLine("Error: missing value for required parameter --part");
            missingParameter = true;
        }
        if (missingParameter) {
            Console.WriteLine("1");
            PrintUsageStringAndExit();
        }
    }
    private static void PrintUsageStringAndExit()
        {
            Console.WriteLine("""
        Usage:

        ./run.sh --day <day> --part <part>
        """);
        Environment.Exit(-1);
        }
}