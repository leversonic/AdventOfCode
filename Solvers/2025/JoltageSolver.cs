namespace AdventOfCode.Solvers._2025;

public class JoltageSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var parsedLines = lines.Select(line => line.Select(c => int.Parse(c.ToString())));
        var sum = 0;
        foreach (var parsedLine in parsedLines)
        {
            var parsedLineList = parsedLine.ToList();
            var largestNumberInfo = GetLargestNumberInfo(parsedLineList);
            if (largestNumberInfo.Index == parsedLineList.Count - 1)
            {
                largestNumberInfo = GetLargestNumberInfo(parsedLineList[..^1]);
            }

            var secondLargestNumberInfo = GetLargestNumberInfo(parsedLineList[(largestNumberInfo.Index + 1)..]);

            var combinedNumber =
                int.Parse($"{largestNumberInfo.Value.ToString()}{secondLargestNumberInfo.Value.ToString()}");
            sum += combinedNumber;
        }

        return sum;

        (int Value, int Index) GetLargestNumberInfo(IEnumerable<int> list) =>
            list.Select((value, index) => (Value: value, Index: index)).MaxBy(tuple => tuple.Value);
    }
}