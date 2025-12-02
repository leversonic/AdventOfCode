using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2025;

public partial class SafeDialPasswordSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var lineRegex = LineRegex();

        var parsedRotations = lines.Parse(lineRegex, ParseDirection, int.Parse);

        const int maxValue = 99;
        var currentValue = 50;
        var zeroCount = 0;

        foreach (var (direction, amount) in parsedRotations)
        {
            Step(direction, amount);
            Console.WriteLine($"Current value: {currentValue}");
            if (part == 1 && currentValue == 0)
            {
                zeroCount++;
            }
        }

        return zeroCount;

        void Step(Direction direction, int amount)
        {
            if (part == 1)
            {
                switch (direction)
                {
                    case Direction.Left:
                        currentValue -= amount;
                        break;
                    case Direction.Right:
                        currentValue += amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction,
                            "Invalid direction encountered");
                }

                switch (currentValue)
                {
                    case > maxValue:
                        while (currentValue > maxValue)
                        {
                            currentValue -= maxValue + 1;
                        }

                        break;
                    case < 0:
                        while (currentValue < 0)
                        {
                            currentValue += maxValue + 1;
                        }

                        break;
                }
            }
            else
            {
                var increment = direction switch
                {
                    Direction.Left => -1,
                    Direction.Right => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction,
                        "Invalid direction encountered")
                };
                for (var i = 0; i < amount; i++)
                {
                    currentValue += increment;
                    currentValue = currentValue switch
                    {
                        > maxValue => 0,
                        < 0 => maxValue,
                        _ => currentValue
                    };

                    if (currentValue == 0)
                    {
                        zeroCount++;
                    }
                }
            }
        }
    }

    private static Direction ParseDirection(string directionString) =>
        directionString switch
        {
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => throw new InvalidOperationException($"Invalid direction: {directionString}")
        };

    private enum Direction
    {
        Left,
        Right,
    }

    [GeneratedRegex("(L|R)(\\d+)")]
    private static partial Regex LineRegex();
}