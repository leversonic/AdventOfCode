using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2024;

public partial class BathroomRobotSolver : ISolver
{
    private const int Width = 101;
    private const int Height = 103;
    private static readonly Stopwatch Stopwatch = new();
    public object Solve(string[] lines, int part)
    {
        var robotInfoRegex = RobotInfoRegex();
        var robots = lines.Select(line =>
        {
            var match = robotInfoRegex.Match(line);
            return new Robot
            {
                Position = new(int.Parse(match.Groups["PositionX"].Value), int.Parse(match.Groups["PositionY"].Value)),
                Velocity = new(int.Parse(match.Groups["VelocityX"].Value), int.Parse(match.Groups["VelocityY"].Value))
            };
        }).ToList();
        if (part == 1)
        {
            const int seconds = 100;
            PrintPicture(robots);
            for (var i = 0; i < seconds; i++)
            {
                Iterate(robots);
                PrintPicture(robots);
            }

            var q1Count = robots.Count(robot => robot.Position.X is >= 0 and < Width / 2 && robot.Position.Y is >= 0 and < Height / 2);
            var q2Count = robots.Count(robot => robot.Position.X is > Width / 2 and < Width && robot.Position.Y is >= 0 and < Height / 2);
            var q3Count = robots.Count(robot => robot.Position.X is >= 0 and < Width / 2 && robot.Position.Y is > Height / 2 and < Height);
            var q4Count = robots.Count(robot => robot.Position.X is > Width / 2 and < Width && robot.Position.Y is > Height / 2 and < Height);
            return q1Count * q2Count * q3Count * q4Count;
        }

        var count = 1;
        Stopwatch.Start();
        Console.CancelKeyPress += delegate
        {
            Stopwatch.Stop();
            // ReSharper disable once AccessToModifiedClosure
            Console.WriteLine($"Evaluated {count} steps");
            Console.WriteLine($"Total runtime: {Stopwatch.Elapsed}");
        };
        while (true)
        {
            Iterate(robots);
            if (IsChristmasTree(robots))
            {
                PrintPicture(robots);
                return count;
            }

            count++;
        }
    }

    private static bool IsChristmasTree(List<Robot> robots)
    {
        const double threshold = 0.5;
        var robotGrid = new bool[Width,Height];
        foreach (var robot in robots)
        {
            robotGrid[robot.Position.X,robot.Position.Y] = true;
        }

        var adjacencyCount = 0;
        var totalCount = 0;
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (robotGrid[x, y])
                {
                    if (HasAdjacency(robotGrid, x, y))
                    {
                        adjacencyCount++;
                    }

                    totalCount++;
                }
            }
        }

        var requiredNumber = threshold * totalCount;
        return adjacencyCount >= requiredNumber;
    }

    private static bool HasAdjacency(bool[,] grid, int x, int y)
    {
        var adjacencyCount = 0;
        for (var xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (var yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0)
                {
                    continue;
                }
                var checkX = x + xOffset;
                var checkY = y + yOffset;
                if (checkX < 0 || checkX >= Width || checkY < 0 || checkY >= Height)
                {
                    continue;
                }

                if (grid[checkX, checkY])
                {
                    adjacencyCount++;
                }
            }
        }

        return adjacencyCount >= 2;
    }

    private static void Iterate(List<Robot> robots)
    {
        foreach (var robot in robots)
        {
            var nextX = robot.Position.X + robot.Velocity.X;
            while (nextX < 0)
            {
                nextX += Width;
            }
            nextX %= Width;
            var nextY = robot.Position.Y + robot.Velocity.Y;
            while (nextY < 0)
            {
                nextY += Height;
            }
            nextY %= Height;
            robot.Position = new(nextX, nextY);
        }
    }

    private static void PrintPicture(List<Robot> robots)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Console.Write(robots.Any(robot => robot.Position.X == x && robot.Position.Y == y) ? '*' : ' ');
            }
            Console.WriteLine();
        }

        Console.WriteLine("------------------------------");
    }

    private class Robot
    {
        public required Coordinate Position { get; set; }
        public required Coordinate Velocity { get; init; }
    }

    [GeneratedRegex(@"p=(?<PositionX>-?\d+),(?<PositionY>-?\d+) v=(?<VelocityX>-?\d+),(?<VelocityY>-?\d+)")]
    private static partial Regex RobotInfoRegex();
}