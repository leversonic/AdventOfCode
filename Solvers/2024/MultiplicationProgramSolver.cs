using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class MultiplicationProgramSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var result = 0;
        if (part == 1)
        {
            var multRegex = MultRegex();
            foreach (var line in lines)
            {
                var multMatches = multRegex.Matches(line).ToList();
                foreach (var match in multMatches)
                {
                    var (a, b) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                    result += a * b;
                }
            }
        }

        return result;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MultRegex();
}