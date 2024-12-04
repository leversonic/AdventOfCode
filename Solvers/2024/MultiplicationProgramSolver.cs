using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class MultiplicationProgramSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var result = 0;
        var commandsRegex = part == 1 ? MultRegex() : AllCommandsRegex();
        var enabled = true;
        foreach (var line in lines)
        {
            var multMatches = commandsRegex.Matches(line).ToList();
            foreach (var match in multMatches)
            {
                if (match.Value.Contains("do()"))
                {
                    enabled = true;
                    continue;
                }

                if (match.Value.Contains("don't()"))
                {
                    enabled = false;
                    continue;
                }

                if (!enabled)
                {
                    continue;
                }

                var (a, b) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                result += a * b;
            }
        }

        return result;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MultRegex();

    [GeneratedRegex(@"(?:mul\((\d+),(\d+)\)|do(n't)?\(\))")]
    private static partial Regex AllCommandsRegex();
}