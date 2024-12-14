using System.Numerics;
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

        double total = 0;

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
        else
        {
            const double conversionError = 10000000000000;
            var bigMachines = machines.Select(m => new Machine<double>
            {
                ButtonA = new(m.ButtonA.X, m.ButtonA.Y),
                ButtonB = new(m.ButtonB.X, m.ButtonB.Y),
                Prize = new Coordinate<double>(m.Prize.X, m.Prize.Y) + conversionError
            });

            foreach (var machine in bigMachines)
            {
                var n = (machine.Prize.Y - machine.Prize.X * machine.ButtonA.Y / machine.ButtonA.X)
                        / (machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X / machine.ButtonA.X);
                var m = (machine.Prize.X - n * machine.ButtonB.X) / machine.ButtonA.X;
                const double tolerance = 0.01;
                if (m > 0 && n > 0
                    && Math.Abs(n - Math.Round(n)) < tolerance
                    && Math.Abs(m - Math.Round(m)) < tolerance)
                {
                    total += m * buttonACost + n * buttonBCost;
                }
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

    private class Machine : Machine<int>;
    private class Machine<T> where T : INumberBase<T>
    {
        public Coordinate<T> ButtonA { get; set; } = null!;
        public Coordinate<T> ButtonB { get; set; } = null!;
        public Coordinate<T> Prize { get; set; } = null!;
    }

    [GeneratedRegex(@"^Button (?<Button>A|B): X\+(?<X>\d+), Y\+(?<Y>\d+)$")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"Prize: X=(?<X>\d+), Y=(?<Y>\d+)$")]
    private static partial Regex PrizeRegex();
}