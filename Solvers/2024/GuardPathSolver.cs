using System.Text;

namespace AdventOfCode.Solvers._2024;

public class GuardPathSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var (currentX, currentY) = lines.Select<string, (int, int)?>((line, index) =>
        {
            var caratIndex = line.IndexOf('^');
            return caratIndex == -1 ? null : (caratIndex, index);
        }).First(index => index != null)!.Value;
        var (startX, startY) = (currentX, currentY);
        var currentDirection = Direction.Up;

        if (part == 1)
        {
            var visitedLocations = new HashSet<(int x, int y)> { (currentX, currentY) };

            while (true)
            {
                var result = ApplyRule(lines, currentX, currentY, currentDirection);
                if (!result.HasValue)
                {
                    return visitedLocations.Count;
                }

                (currentX, currentY, currentDirection) = result.Value;
                visitedLocations.Add((currentX, currentY));
            }
        }

        var loopCount = 0;
        for (var obstacleX = 0; obstacleX < lines[0].Length; obstacleX++)
        {
            for (var obstacleY = 0; obstacleY < lines.Length; obstacleY++)
            {
                if (obstacleX == startX && obstacleY == startY || lines[obstacleY][obstacleX] == '#')
                {
                    continue;
                }

                (currentX, currentY, currentDirection) = (startX, startY, Direction.Up);
                var linesWithObstacle = lines.Select(line => line).ToArray();
                var sb = new StringBuilder(linesWithObstacle[obstacleY])
                {
                    [obstacleX] = '#'
                };
                linesWithObstacle[obstacleY] = sb.ToString();
                var visitedLocationsWithDirection = new HashSet<(int x, int y, Direction dir)>{ (currentX, currentY, currentDirection)};
                while (true)
                {
                    var result = ApplyRule(linesWithObstacle, currentX, currentY, currentDirection);
                    if (!result.HasValue)
                    {
                        break;
                    }

                    (currentX, currentY, currentDirection) = result.Value;
                    if (visitedLocationsWithDirection.Add(result.Value))
                    {
                        continue;
                    }

                    loopCount++;
                    break;
                }
            }
        }
        return loopCount;
    }

    private static (int x, int y, Direction direction)? ApplyRule(string[] lines, int currentX, int currentY, Direction currentDirection)
    {
        if (IsLeavingMap(lines[0].Length, lines.Length, currentX, currentY, currentDirection))
        {
            return null;
        }

        if (IsObstacleInDirection(lines, currentX, currentY, currentDirection))
        {
            return (currentX, currentY, TurnRight(currentDirection));
        }

        var (newX, newY) = Move(currentX, currentY, currentDirection);
        return (newX, newY, currentDirection);
    }

    private static bool IsLeavingMap(int xSize, int ySize, int x, int y, Direction direction) =>
        direction switch
        {
            Direction.Up => y <= 0,
            Direction.Left => x <= 0,
            Direction.Down => y >= ySize - 1,
            Direction.Right => x >= xSize - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };

    private static bool IsObstacleInDirection(string[] map, int x, int y, Direction direction) =>
        direction switch
        {
            Direction.Up => map[y - 1][x] == '#',
            Direction.Left => map[y][x - 1] == '#',
            Direction.Down => map[y + 1][x] == '#',
            Direction.Right => map[y][x + 1] == '#',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };

    private static Direction TurnRight(Direction direction) =>
        direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };

    private static (int x, int y) Move(int x, int y, Direction direction) =>
        direction switch
        {
            Direction.Up => (x, y - 1),
            Direction.Left => (x - 1, y),
            Direction.Down => (x, y + 1),
            Direction.Right => (x + 1, y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}