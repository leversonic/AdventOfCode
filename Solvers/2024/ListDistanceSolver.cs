﻿using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class ListDistanceSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var lineRegex = LineRegex();
        var lineValues = lines.Select(line =>
        {
            var match = lineRegex.Match(line);
            return (match.Groups[1].Value, match.Groups[2].Value);
        }).ToList();

        var (leftValues, rightValues) = (lineValues.Select(v => int.Parse(v.Item1)).Order().ToList(),
            lineValues.Select(v => int.Parse(v.Item2)).Order().ToList());
        var total = 0;
        for (var i = 0; i < leftValues.Count; i++)
        {
            var left = leftValues[i];
            var right = rightValues[i];
            if (part == 1)
            {
                total += Math.Abs(left - right);
            }
            else
            {
                total += left * rightValues.Count(v => v == left);
            }
        }

        return total;
    }

    [GeneratedRegex(@"^\s*(\d+)\s*(\d+)$")]
    private static partial Regex LineRegex();
}