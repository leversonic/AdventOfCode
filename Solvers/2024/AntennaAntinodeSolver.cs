using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class AntennaAntinodeSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var antennaRegex = AntennaRegex();

        var antennaLocations = lines
            .SelectMany((line, y) => line
                .Select<char, (int x, int y, char c)?>((c, x) => antennaRegex.IsMatch($"{c}")
                    ? (x, y, c) 
                    : null)
                .Where(tuple => tuple.HasValue))
            .Select(v => v!.Value)
            .ToList();
        var antinodes = new HashSet<(int x, int y)>();

            foreach (var location in antennaLocations)
            {
                var otherLocations = antennaLocations
                    .Where(l => l != location && l.c == location.c);
                foreach (var otherLocation in otherLocations)
                {
                    var difference = (x: location.x - otherLocation.x, y: location.y - otherLocation.y);
                    if (part == 1)
                    {
                        var antinode1 = (x: location.x + difference.x, y: location.y + difference.y);
                        var antinode2 = (x: otherLocation.x - difference.x, y: otherLocation.y - difference.y);
                        new[] { antinode1, antinode2 }
                            .Where(InRange)
                            .ToList()
                            .ForEach(antinode => antinodes.Add(antinode));
                    }
                    else
                    {
                        var (xFactor, yFactor) = DivideByLeastCommonPrimeFactor(difference.x, difference.y);
                        var multiple = 0;
                        while (true)
                        {
                            var positiveLocationToTest = (x: otherLocation.x + xFactor * multiple, y: otherLocation.y + yFactor * multiple);
                            var negativeLocationToTest = (x: location.x - xFactor * multiple, y: location.y - yFactor * multiple);
                            var positiveInRange = InRange(positiveLocationToTest);
                            var negativeInRange = InRange(negativeLocationToTest);
                            if (positiveInRange)
                            {
                                antinodes.Add(positiveLocationToTest);
                            }

                            if (negativeInRange)
                            {
                                antinodes.Add(negativeLocationToTest);
                            }

                            multiple++;
                            if (!positiveInRange && !negativeInRange)
                            {
                                break;
                            }
                        }
                    }
                }
            }

        return antinodes.Count;

        bool InRange((int x, int y) antinode) => 
            antinode.x >= 0 && antinode.x < lines[0].Length && antinode.y >= 0 && antinode.y < lines.Length;

        (int n1, int n2) DivideByLeastCommonPrimeFactor(int n1, int n2)
        {
            for (var i = 2; i <= n1; i++)
            {
                if (n1 % i == 0 && n2 % i == 0) return (n1 / i, n2 / i);
            }
            return (n1, n2);
        }
    }

    [GeneratedRegex("[a-zA-Z0-9]{1}")]
    private static partial Regex AntennaRegex();
}