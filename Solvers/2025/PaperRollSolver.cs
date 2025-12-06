namespace AdventOfCode.Solvers._2025;

public class PaperRollSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var xMax = lines[0].Length - 1;
        var yMax = lines.Length - 1;
        var count = 0;
        int currentIterationCount;
        var positionsToRemove = new List<(int X, int Y)>();
        do
        {
            currentIterationCount = 0;
            for (var x = 0; x <= xMax; x++)
            {
                for (var y = 0; y <= yMax; y++)
                {
                    if (lines[y][x] != '@')
                    {
                        continue;
                    }

                    var positions = GetValidPositions(x, y);
                    if (positions.Sum(tuple => lines[tuple.Y][tuple.X] == '@' ? 1 : 0) < 4)
                    {
                        count++;
                        currentIterationCount++;
                        positionsToRemove.Add((x, y));
                    }
                }
            }

            foreach (var (x, y) in positionsToRemove)
            {
                var updatedLineChars = lines[y].ToCharArray();
                updatedLineChars[x] = '.';
                lines[y] = new(updatedLineChars);
            }
        } while (currentIterationCount > 0 && part == 2);

        return count;

        IEnumerable<(int X, int Y)> GetValidPositions(int x, int y)
        {
            return new List<(int X, int Y)>
                {
                    (x - 1, y - 1),
                    (x - 1, y),
                    (x - 1, y + 1),
                    (x, y + 1),
                    (x, y - 1),
                    (x + 1, y - 1),
                    (x + 1, y),
                    (x + 1, y + 1)
                }
                .Where(tuple => tuple.X >= 0 && tuple.X <= xMax && tuple.Y >= 0 && tuple.Y <= yMax);
        }
    }
}