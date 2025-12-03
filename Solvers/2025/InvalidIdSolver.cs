using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2025;

public partial class InvalidIdSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1)
        {
            var ranges = lines[0].Split(',').Parse(
                IdRangeRegex(),
                id => (id, BigInteger.Parse(id)),
                id => (id, BigInteger.Parse(id)));

            var counter = BigInteger.Zero;

            foreach (var ((startText, startNumber), (endText, endNumber)) in ranges)
            {
                var localStartText = startText;
                var localStartNumber = startNumber;
                var localEndText = endText;
                var localEndNumber = endNumber;
                if (startText.Length != endText.Length)
                {
                    if (startText.Length % 2 != 0)
                    {
                        localStartText = $"1{string.Concat(Enumerable.Repeat('0', startText.Length))}";
                        localStartNumber = BigInteger.Parse(localStartText);
                    }

                    if (endText.Length % 2 != 0)
                    {
                        localEndText = $"{string.Concat(Enumerable.Repeat('9', endText.Length - 1))}";
                        localEndNumber = BigInteger.Parse(localEndText);
                    }
                }
                else if (startText.Length % 2 != 0)
                {
                    continue;
                }

                counter += CheckRange(localStartText, localStartNumber, localEndText, localEndNumber);
            }

            return counter;
        }

        throw new NotImplementedException("Part 2 not yet implemented");
    }

    private static BigInteger CheckRange(string startText, BigInteger startNumber, string endText, BigInteger endNumber)
    {
        var counter = BigInteger.Zero;
        var startFirstHalf = startText[..(startText.Length / 2)];
        var endFirstHalf = endText[..(endText.Length / 2)];
        for (var i = int.Parse(startFirstHalf); i <= int.Parse(endFirstHalf); i++)
        {
            var iString = i.ToString();
            var candidateNumber = BigInteger.Parse($"{iString}{iString}");
            if (candidateNumber < startNumber || candidateNumber > endNumber)
            {
                continue;
            }

            counter += candidateNumber;
            Console.WriteLine($"Found invalid ID: {candidateNumber}");
        }

        return counter;
    }

    [GeneratedRegex(@"(\d+)-(\d+)")]
    private static partial Regex IdRangeRegex();
}