using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2025;

public partial class IngredientFreshnessSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var availableIngredientsStartIndex = -1;
        for (var i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            availableIngredientsStartIndex = i + 1;
            break;
        }

        var freshIdRanges = lines[..(availableIngredientsStartIndex - 1)]
            .Parse(RangeRegex(), BigInteger.Parse, BigInteger.Parse)
            .Select(tuple => (Start: tuple.Item1, End: tuple.Item2))
            .ToList();

        var availableIds = lines[availableIngredientsStartIndex..].Parse(BigInteger.Parse);

        return availableIds
            .Select(i => i.Item1)
            .Count(id => freshIdRanges.Any(range => range.Start <= id && range.End >= id));
    }

    [GeneratedRegex(@"(\d+)-(\d+)")]
    private static partial Regex RangeRegex();
}