using System.Numerics;

namespace AdventOfCode.Solvers._2025;

public class TachyonManifoldSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var charLines = lines.Select(line => line.ToCharArray()).ToArray();
        var currentPathsCountDict = new Dictionary<int, BigInteger>
        {
            [Array.IndexOf(charLines[0], 'S')] = 1
        };
        var splitCount = 0;
        foreach (var line in charLines)
        {
            var nextPathsCountDict = new Dictionary<int, BigInteger>();
            foreach (var beamIndex in currentPathsCountDict.Keys)
            {
                if (line[beamIndex] == '^')
                {
                    if (beamIndex > 0)
                    {
                        line[beamIndex - 1] = '|';
                        nextPathsCountDict.TryGetValue(beamIndex - 1, out var existingPathCount);
                        nextPathsCountDict[beamIndex - 1] = existingPathCount + currentPathsCountDict[beamIndex];
                    }

                    if (beamIndex < charLines.Length - 1)
                    {
                        line[beamIndex + 1] = '|';
                        nextPathsCountDict.TryGetValue(beamIndex + 1, out var existingPathCount);
                        nextPathsCountDict[beamIndex + 1] = existingPathCount + currentPathsCountDict[beamIndex];
                    }

                    splitCount++;
                }
                else
                {
                    line[beamIndex] = '|';
                    nextPathsCountDict.TryGetValue(beamIndex, out var existingPathCount);
                    nextPathsCountDict[beamIndex] = existingPathCount + currentPathsCountDict[beamIndex];
                }
            }

            currentPathsCountDict = nextPathsCountDict;
        }

        if (part == 1)
        {
            return splitCount;
        }

        if (part == 2)
        {
            return currentPathsCountDict.Values.Aggregate((acc, next) => acc + next);
        }

        return -1;
    }
}