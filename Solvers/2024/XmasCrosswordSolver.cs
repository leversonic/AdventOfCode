namespace AdventOfCode.Solvers._2024;

public class XmasCrosswordSolver : ISolver
{
    private string[] _lines = [];
    private int LinesHeight => _lines.Length;
    private int LinesWidth => _lines.Length == 0 ? 0 : _lines[0].Length;
    private const string WordToFind = "XMAS";
    public object Solve(string[] lines, int part)
    {
        _lines = lines;
        var total = 0;
        if (part == 1)
        {
            for (var x = 0; x < LinesWidth; x++)
            {
                for (var y = 0; y < LinesHeight; y++)
                {
                    if (_lines[y][x] == WordToFind[0])
                    {
                        total += new[] { -1, 0, 1 }
                            .Sum(xDir => new[] { -1, 0, 1 }
                                .Sum(yDir => GetCount(x, y, xDir, yDir)));
                    }
                }
            }
        }

        return total;
    }

    private int GetCount(int x, int y, int xDir, int yDir)
    {
        var currentLetter = _lines[y][x];
        var currentLetterIndex = WordToFind.IndexOf(currentLetter);
        if (currentLetterIndex == WordToFind.Length - 1)
        {
            return 1;
        }
        var nextX = x + xDir;
        var nextY = y + yDir;
        if (nextX < 0 || nextX >= LinesWidth)
        {
            return 0;
        }
        if (nextY < 0 || nextY >= LinesHeight)
        {
            return 0;
        }
        var nextLetterIndex = WordToFind.IndexOf(_lines[nextY][nextX]);
        // ReSharper disable once TailRecursiveCall
        return nextLetterIndex == currentLetterIndex + 1 ? GetCount(nextX, nextY, xDir, yDir) : 0;
    }
}