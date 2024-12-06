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
            foreach (var pageLine in pageLines)
            {
                var lineIsValid = ApplyRules(ruleLines, pageLine);
                switch (part)
                {
                    case 2 when !lineIsValid:
                    {
                        var finalPageLine = FixLine(ruleLines, pageLine);
                        total += int.Parse(finalPageLine[finalPageLine.Count / 2]);
                        break;
                    }
                    case 1 when lineIsValid:
                        total += int.Parse(pageLine[pageLine.Count / 2]);
                        break;
                }
            }

        return total;
    }

    private static bool ApplyRules((string, string)[] ruleLines, List<string> pageLine) =>
        ruleLines.All(ruleLine => ApplyRule(ruleLine, pageLine));

    private static List<string> FixLine((string, string)[] ruleLines, List<string> pageLine)
    {
        var result = pageLine.Select(l => l).ToList();
        while (true)
        {
            var isFixed = true;
            foreach (var ruleLine in ruleLines)
            {
                if (ApplyRule(ruleLine, result))
                {
                    continue;
                }
                isFixed = false;
                result.RemoveAt(result.IndexOf(ruleLine.Item2));
                result.Insert(result.IndexOf(ruleLine.Item1) + 1, ruleLine.Item2);
            }

            if (isFixed)
            {
                break;
            }
        }

        return result;
    }

    private static bool ApplyRule((string, string) ruleLine, List<string> pageLine)
    {
        var firstItemIndex = pageLine.IndexOf(ruleLine.Item1);
        var secondItemIndex = pageLine.IndexOf(ruleLine.Item2);
        return firstItemIndex == -1 || secondItemIndex == -1 || firstItemIndex < secondItemIndex;
    }
}