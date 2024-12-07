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

        if (part == 1)
        {
            var visitedLocations = new HashSet<(int x, int y)> { (currentX, currentY) };
            var currentDirection = Direction.Up;

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

        return null;
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