using System.Text.RegularExpressions;
using AdventOfCode.Utility;

namespace AdventOfCode.Solvers._2024;

public partial class BathroomRobotSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        const int width = 101;
        const int height = 103;

        var robotInfoRegex = RobotInfoRegex();
        var robots = lines.Select(line =>
        {
            var match = robotInfoRegex.Match(line);
            return new Robot
            {
                Position = new(int.Parse(match.Groups["PositionX"].Value), int.Parse(match.Groups["PositionY"].Value)),
                Velocity = new(int.Parse(match.Groups["VelocityX"].Value), int.Parse(match.Groups["VelocityY"].Value)),
            };
        }).ToList();
        if (part == 1)
        {
            const int seconds = 100;
            for (var i = 0; i < seconds; i++)
            {
                foreach (var robot in robots)
                {
                    var nextX = robot.Position.X + robot.Velocity.X;
                    while (nextX < 0)
                    {
                        nextX += width;
                    }
                    nextX %= width;
                    var nextY = robot.Position.Y + robot.Velocity.Y;
                    while (nextY < 0)
                    {
                        nextY += height;
                    }
                    nextY %= height;
                    robot.Position = new(nextX, nextY);
                }
            }

            var q1Count = robots.Count(robot => robot.Position.X is >= 0 and < width / 2 && robot.Position.Y is >= 0 and < height / 2);
            var q2Count = robots.Count(robot => robot.Position.X is > width / 2 and < width && robot.Position.Y is >= 0 and < height / 2);
            var q3Count = robots.Count(robot => robot.Position.X is >= 0 and < width / 2 && robot.Position.Y is > height / 2 and < height);
            var q4Count = robots.Count(robot => robot.Position.X is > width / 2 and < width && robot.Position.Y is > height / 2 and < height);
            return q1Count * q2Count * q3Count * q4Count;
        }

        return 0;
    }

    private class Robot
    {
        public Coordinate Position { get; set; }
        public Coordinate Velocity { get; set; }
    }

    [GeneratedRegex(@"p=(?<PositionX>-?\d+),(?<PositionY>-?\d+) v=(?<VelocityX>-?\d+),(?<VelocityY>-?\d+)")]
    private static partial Regex RobotInfoRegex();
}