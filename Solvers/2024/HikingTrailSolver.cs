﻿namespace AdventOfCode.Solvers._2024;

public class HikingTrailSolver : ISolver
{
    private List<(int x, int y)> _visitedPeaks = [];
    public object Solve(string[] lines, int part)
    {
        var map = lines.Select(line => line.Select(c => int.Parse($"{c}")).ToArray()).ToArray();
        
        var trailheads = map
            .SelectMany((line, y) => line
                .Select((_, x) => (x, y)))
            .Where(tuple => map[tuple.y][tuple.x] == 0);
        var score = 0;
        foreach (var trailhead in trailheads)
        {
            _visitedPeaks = [];
            score += CalculateScore(map, trailhead.x, trailhead.y, part == 1);
        }

        return score;
    }

    private int CalculateScore(int[][] map, int x, int y, bool trackVisited)
    {
        var currentValue = map[y][x];
        var score = 0;
        if (x > 0)
        {
            if (map[y][x - 1] == currentValue + 1)
            {
                if (currentValue == 8 && (!trackVisited || !_visitedPeaks.Contains((x - 1, y))))
                {
                    _visitedPeaks.Add((x - 1, y));
                    score += 1;
                }
                score += CalculateScore(map, x - 1, y, trackVisited);
            }
        }
        if (x < map.Length - 1)
        {
            if (map[y][x + 1] == currentValue + 1)
            {
                if (currentValue == 8 && (!trackVisited || !_visitedPeaks.Contains((x + 1, y))))
                {
                    _visitedPeaks.Add((x + 1, y));
                    score += 1;
                }
                score += CalculateScore(map, x + 1, y, trackVisited);
            }
        }
        if (y > 0)
        {
            if (map[y - 1][x] == currentValue + 1)
            {
                if (currentValue == 8 && (!trackVisited || !_visitedPeaks.Contains((x, y - 1))))
                {
                    _visitedPeaks.Add((x, y - 1));
                    score += 1;
                }
                score += CalculateScore(map, x, y - 1, trackVisited);
            }
        }
        if (y < map.Length - 1)
        {
            if (map[y + 1][x] == currentValue + 1)
            {
                if (currentValue == 8 && (!trackVisited || !_visitedPeaks.Contains((x, y + 1))))
                {
                    _visitedPeaks.Add((x, y + 1));
                    score += 1;
                }
                score += CalculateScore(map, x, y + 1, trackVisited);
            }
        }
        
        return score;
    }
}