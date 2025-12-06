using System.Numerics;
using System.Text;
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

        switch (part)
        {
            case 1:
            {
                var availableIds = lines[availableIngredientsStartIndex..].Parse(BigInteger.Parse);

                return availableIds
                    .Select(i => i.Item1)
                    .Count(id => freshIdRanges.Any(range => range.Start <= id && range.End >= id));
            }
            case 2:
            {
                var orderedRanges = freshIdRanges
                    .OrderBy(range => range.Start)
                    .ThenBy(range => range.End)
                    .ToList();
                var mergedRanges = new List<(BigInteger Start, BigInteger End)>();
                var accumulatingRange = orderedRanges.First();
                foreach (var range in orderedRanges.Skip(1))
                {
                    if (range.Start > accumulatingRange.End)
                    {
                        mergedRanges.Add(accumulatingRange);
                        accumulatingRange = range;
                        continue;
                    }

                    var newStart = BigInteger.Min(accumulatingRange.Start, range.Start);
                    var newEnd = BigInteger.Max(accumulatingRange.End, range.End);
                    Console.WriteLine(
                        $"Merging ranges ({accumulatingRange.Start},{accumulatingRange.End}) and ({range.Start},{range.End}) into new range ({newStart},{newEnd})");
                    accumulatingRange = (newStart, newEnd);
                }

                if (mergedRanges.Last() != accumulatingRange)
                {
                    mergedRanges.Add(accumulatingRange);
                }

                var sb = new StringBuilder("{\n");
                foreach (var range in mergedRanges)
                {
                    sb.Append($"\t({range.Start},{range.End})\n");
                }

                sb.Append('}');

                Console.WriteLine("Ranges:");
                Console.WriteLine(sb.ToString());

                return mergedRanges.Select(range => range.End - range.Start + 1).Aggregate((acc, next) => acc + next);
            }
            default:
                return -1;
        }
    }

    [GeneratedRegex(@"(\d+)-(\d+)")]
    private static partial Regex RangeRegex();
}