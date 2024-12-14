using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2024;

public partial class ClawMachineSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        const int buttonACost = 3;
        const int buttonBCost = 1;
        var machines = ParseLines(lines);

        var total = 0;

        if (part == 1)
        {
            foreach (var machine in machines)
            {
                var options = new List<(int a, int b)>();
                for (var a = 0; a <= 100; a++)
                {
                    for (var b = 0; b <= 100; b++)
                    {
                        if ((a * machine.ButtonA + b * machine.ButtonB).Equals(machine.Prize))
                        {
                            options.Add((a, b));
                        }
                    }
                }
                total += options.Count > 0
                    ? options.Min(option => option.a * buttonACost + option.b * buttonBCost)
                    : 0;
            }
        }

        return total;
    }

    private static List<Machine> ParseLines(string[] lines)
    {
        var lineIndex = 0;
        var buttonRegex = ButtonRegex();
        var prizeRegex = PrizeRegex();
        var currentMachine = new Machine();
        var machines = new List<Machine>();
        while (lineIndex < lines.Length)
        {
            var line = lines[lineIndex];
            var buttonMatch = buttonRegex.Match(line);
            var prizeMatch = prizeRegex.Match(line);
            if (buttonMatch.Success)
            {
                var button = buttonMatch.Groups["Button"].Value;
                var x = int.Parse(buttonMatch.Groups["X"].Value);
                var y = int.Parse(buttonMatch.Groups["Y"].Value);
                switch (button)
                {
                    case "A":
                        currentMachine.ButtonA = new(x, y);
                        break;
                    case "B":
                        currentMachine.ButtonB = new(x, y);
                        break;
                }
            } else if (prizeMatch.Success)
            {
                var x = int.Parse(prizeMatch.Groups["X"].Value);
                var y = int.Parse(prizeMatch.Groups["Y"].Value);
                currentMachine.Prize = new(x, y);
                if (lineIndex == lines.Length - 1)
                {
                    machines.Add(currentMachine);
                }
            }
            else
            {
                machines.Add(currentMachine);
                currentMachine = new();
            }
            lineIndex++;
        }

        return machines;
    }

    private class Machine
    {
        public Coordinate ButtonA { get; set; } = new(0, 0);
        public Coordinate ButtonB { get; set; } = new(0, 0);
        public Coordinate Prize { get; set; } = new(0, 0);
    }

    [GeneratedRegex(@"^Button (?<Button>A|B): X\+(?<X>\d+), Y\+(?<Y>\d+)$")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"Prize: X=(?<X>\d+), Y=(?<Y>\d+)$")]
    private static partial Regex PrizeRegex();
}