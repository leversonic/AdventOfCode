using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2024;

public class WarehouseRobotSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var warehouseConfigLines = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        var moveLines = lines[warehouseConfigLines.Length..];
        var moves = moveLines.SelectMany(line => line.Select(ToDirection));
        var robotLocation = warehouseConfigLines
            .Select((_, index) => index)
            .Select(y =>
            {
                var x = warehouseConfigLines[y].IndexOf('@');
                return x == -1 ? null : new Coordinate(x, y);
            })
            .FirstOrDefault(index => index != null)!;

        foreach (var move in moves)
        {
            Coordinate nextLocation = move switch
            {
                Direction.Up => new(robotLocation.X, robotLocation.Y - 1),
                Direction.Down => new(robotLocation.X, robotLocation.Y + 1),
                Direction.Left => new(robotLocation.X - 1, robotLocation.Y),
                Direction.Right => new(robotLocation.X + 1, robotLocation.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(nextLocation), move, "Invalid move direction.")
            };
            if (Move(warehouseConfigLines, robotLocation, move))
            {
                robotLocation = nextLocation;
            }
        }

        var total = warehouseConfigLines
            .Select((_, index) => index)
            .Select(y =>
            {
                var lineSum = 0;
                for (var x = 0; x < warehouseConfigLines[y].Length; x++)
                {
                    if (warehouseConfigLines[y][x] == 'O')
                    {
                        lineSum += 100 * y + x;
                    }
                }

                return lineSum;
            }).Sum();

        return total;
    }

    private static bool Move(string[] warehouseConfig, Coordinate objectPosition, Direction moveDirection)
    {
        var currentObject = warehouseConfig[objectPosition.Y][objectPosition.X];
        switch (currentObject)
        {
            case '.':
                return true;
            case '#':
                return false;
        }

        Coordinate nextPosition = moveDirection switch
        {
            Direction.Up => new(objectPosition.X, objectPosition.Y - 1),
            Direction.Down => new(objectPosition.X, objectPosition.Y + 1),
            Direction.Left => new(objectPosition.X - 1, objectPosition.Y),
            Direction.Right => new(objectPosition.X + 1, objectPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null)
        };
        if (currentObject is 'O' or '@' && Move(warehouseConfig, nextPosition, moveDirection))
        {
            var replacementLine = warehouseConfig[nextPosition.Y].ToCharArray();
            replacementLine[nextPosition.X] = currentObject;
            warehouseConfig[nextPosition.Y] = new(replacementLine);
            replacementLine = warehouseConfig[objectPosition.Y].ToCharArray();
            replacementLine[objectPosition.X] = '.';
            warehouseConfig[objectPosition.Y] = new(replacementLine);
            return true;
        }

        return false;
    }

    private static Direction ToDirection(char directionChar) =>
        directionChar switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new ArgumentException($"Invalid direction character: {directionChar}")
        };

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };
}