namespace AdventOfCode.Solvers._2024;

public class SafetyManualPageSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var total = 0;
        var breakIndex = lines.ToList().FindIndex(string.IsNullOrWhiteSpace);
        var ruleLines = lines[..breakIndex].Select(line =>
        {
            var splitLine = line.Split('|');
            return (splitLine[0], splitLine[1]);
        }).ToArray();
        var pageLines = lines[(breakIndex + 1)..].Select(line => line.Split(',').ToList()).ToList();
        if (part == 1)
        {
            foreach (var pageLine in pageLines)
            {
                var lineIsValid = ApplyRules(ruleLines, pageLine);
                if (lineIsValid)
                {
                    total += int.Parse(pageLine[pageLine.Count / 2]);
                }
            }
        }

        return total;
    }

    private static bool ApplyRules((string, string)[] ruleLines, List<string> pageLine) =>
        ruleLines.All(ruleLine =>
        {
            var firstItemIndex = pageLine.IndexOf(ruleLine.Item1);
            var secondItemIndex = pageLine.IndexOf(ruleLine.Item2);
            return firstItemIndex == -1 || secondItemIndex == -1 || firstItemIndex < secondItemIndex;
        });
}