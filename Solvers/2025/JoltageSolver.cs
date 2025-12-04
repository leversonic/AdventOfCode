using System.Numerics;

namespace AdventOfCode.Solvers._2025;

public class JoltageSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var parsedLines = lines.Select(line => line.Select(c => int.Parse(c.ToString())));
        var sum = BigInteger.Zero;
        var digitCount = part switch
        {
            1 => 2,
            2 => 12,
            _ => throw new InvalidOperationException("Invalid part argument encountered")
        };
        foreach (var parsedLine in parsedLines)
        {
            var parsedLineList = parsedLine.ToList();
            var remainingDigitsCount = digitCount - 1;
            var currentDigits = new List<int>();
            while (remainingDigitsCount >= 0)
            {
                var nextDigitInfo = GetLargestNumberInfo(parsedLineList, remainingDigitsCount);
                currentDigits.Add(nextDigitInfo.Value);
                parsedLineList = parsedLineList[(nextDigitInfo.Index + 1)..];
                remainingDigitsCount--;
            }

            var combinedNumber = BigInteger.Parse(string.Join("", currentDigits));
            sum += combinedNumber;
        }

        return sum;

        (int Value, int Index) GetLargestNumberInfo(IEnumerable<int> list, int digitsToOmit) =>
            list.ToList()[..^digitsToOmit]
                .Select((value, index) => (Value: value, Index: index))
                .MaxBy(tuple => tuple.Value);
    }
}