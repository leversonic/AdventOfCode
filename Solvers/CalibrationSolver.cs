using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

partial class CalibrationSolver : ISolver
{
    public int Solve(string[] lines, int part)
    {
        if (part == 1) {
            var regex = CalibrationPart1Regex();

            return lines.Sum(line => {
                var matches = regex.Matches(line);
                return int.Parse(matches.First().Value + matches.Last().Value);
            });
        }

        throw new ArgumentException("Part 2 not implemented");
    }

    [GeneratedRegex("\\d")]
    private static partial Regex CalibrationPart1Regex();
}