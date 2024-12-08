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

        var result = 0;
        if (part == 1)
        {
            foreach (var location in antennaLocations)
            {
                var otherLocations = antennaLocations
                    .Where(l => l != location && l.c == location.c);
                foreach (var otherLocation in otherLocations)
                {
                    var difference = (location.x - otherLocation.x, location.y - otherLocation.y);
                    (int x, int y) antinode1 = (location.x + difference.Item1, location.y + difference.Item2);
                    (int x, int y) antinode2 = (otherLocation.x - difference.Item1, otherLocation.y - difference.Item2);
                    new[] { antinode1, antinode2 }
                        .Where(InRange)
                        .ToList()
                        .ForEach(antinode => antinodes.Add(antinode));
                }
            }

            result = antinodes.Count;
        }

        return result;

        bool InRange((int x, int y) antinode) => 
            antinode.x >= 0 && antinode.x < lines[0].Length && antinode.y >= 0 && antinode.y < lines.Length;
    }

    [GeneratedRegex("[a-zA-Z0-9]{1}")]
    private static partial Regex AntennaRegex();
}