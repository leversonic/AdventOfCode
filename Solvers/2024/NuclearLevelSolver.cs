namespace AdventOfCode.Solvers._2024;

public class NuclearLevelSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var values = lines.Select(line => line.Split(" ").Select(int.Parse).ToArray()).ToArray();
        if (part == 1)
        {
            return values.Count(value =>
            {
                var sign = 0;
                for (var i = 0; i < value.Length - 1; i++)
                {
                    var difference = value[i + 1] - value[i];
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
            });
        }
        else
        {
            return null!;
        }
    }
}