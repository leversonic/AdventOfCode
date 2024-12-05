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
        for (var x = 0; x < LinesWidth; x++)
        {
            for (var y = 0; y < LinesHeight; y++)
            {
                if (part == 1)
                {
                    if (_lines[y][x] == WordToFind[0])
                    {
                        total += new[] { -1, 0, 1 }
                            .Sum(xDir => new[] { -1, 0, 1 }
                                .Sum(yDir => GetCount(x, y, xDir, yDir)));
                    }
                }
                else
                {
                    if (CheckForXmas(x, y))
                    {
                        total++;
                    }
                }
            }
        }

        return total;
    }

    private bool CheckForXmas(int x, int y)
    {
        if (x - 1 < 0 || x + 1 >= LinesWidth || y - 1 < 0 || y + 1 >= LinesHeight || _lines[y][x] != 'A')
        {
            return false;
        }

        var leg1Chars = new []{_lines[y - 1][x - 1], _lines[y + 1][x + 1]}
            .Order()
            .Aggregate("", (c1, c2) => c1+c2);
        var leg2Chars = new []{_lines[y - 1][x + 1], _lines[y + 1][x - 1]}
            .Order()
            .Aggregate("", (c1, c2) => c1+c2);

        return leg1Chars == leg2Chars && leg2Chars == "MS";
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