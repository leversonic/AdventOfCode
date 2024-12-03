namespace AdventOfCode.Solvers._2024;

public class NuclearLevelSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var values = lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToArray();
        if (part == 1)
        {
            return values.Count(IsRowSafe);
        }

        return values.Count(row =>
        {
            return row.Select((_, index) =>
            {
                var copy = row.Select(v => v).ToList();
                copy.RemoveAt(index);
                return copy;
            }).Any(IsRowSafe);
        });
    }

    private static bool IsRowSafe(List<int> row)
    {
        var sign = 0;
        for (var i = 0; i < row.Count - 1; i++)
        {
            var difference = row[i + 1] - row[i];
            var newSign = difference < 0 ? -1 : 1;
            if (sign != 0 && newSign != sign)
            {
                return false;
            }

            if (Math.Abs(difference) is < 1 or > 3)
            {
                return false;
            }

            sign = newSign;
        }

        return true;
    }
}