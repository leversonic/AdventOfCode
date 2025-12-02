namespace AdventOfCode;

public static class Program
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

        var year = -1;
        var day = -1;
        var part = -1;
        var testMode = false;

        foreach (var (argName, index) in argIndices)
        {
            switch (argName)
            {
                case "--test":
                    testMode = true;
                    continue;
            }

            var argIndex = index + 1;
            if (argIndex >= args.Length)
            {
                PrintUsageStringAndExit();
            }

            var argValue = args[argIndex];
            switch (argName)
            {
                case "--year":
                    if (!int.TryParse(argValue, out year))
                    {
                        PrintUsageStringAndExit();
                        return;
                    }

                    break;
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
            }
        }

        var missingParameter = false;
        if (year == -1)
        {
            Console.WriteLine("Error: missing value for required parameter --year");
            missingParameter = true;
        }

        if (day == -1)
        {
            Console.WriteLine("Error: missing value for required parameter --day");
            missingParameter = true;
        }

        if (part == -1)
        {
            Console.WriteLine("Error: missing value for required parameter --part");
            missingParameter = true;
        }

        if (missingParameter)
        {
            PrintUsageStringAndExit();
        }

        var lines = File.ReadAllLines($"./input/{year:D}/{day:D2}{GetTestString()}.txt");

        try
        {
            var solver = SolverDict.Solvers[year][day];
            var solution = solver.Solve(lines, part);

            Console.WriteLine($"Solution: {solution}");
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine(
                $"Error: solution for Day {day} Part {part} either has not been implemented or has not been added to SolverDict.");
        }

        return;

        string GetTestString() => testMode ? "-test" : "";
    }

    private static void PrintUsageStringAndExit()
    {
        Console.WriteLine("""
                          Usage:

                          ./run.sh --day <day> --part <part> --year <year> (--test)
                          """);
        Environment.Exit(-1);
    }
}