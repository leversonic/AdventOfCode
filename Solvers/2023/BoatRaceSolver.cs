using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

public partial class BoatRaceSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var timesGroup = TimeRegex().Match(lines[0]).Groups[1].Value.Trim();
        var timeMatches = NumberRegex().Matches(timesGroup).Select(m => m.Value).ToArray();
        var distanceGroup = DistanceRegex().Match(lines[1]).Groups[1].Value.Trim();
        var distanceMatches = NumberRegex().Matches(distanceGroup).Select(m => m.Value).ToArray();
        if (part == 1) {
            (int time, int distance)[] records = timeMatches
                .Select((m, i) => (time: int.Parse(m), distance: int.Parse(distanceMatches[i])))
                .ToArray();

            return records
                .Select(record => {
                    var total = 0;
                    for(var i = 0; i < record.time; i++) {
                        if (i * (record.time - i) > record.distance) {
                            total++;
                        }
                    }
                    return total;
                })
                .Aggregate((acc, next) => acc * next);
        }
        else
        {
            BigInteger recordTime = BigInteger.Parse(
                timeMatches.Aggregate((acc, next) => acc + next)
            );
            BigInteger recordDistance = BigInteger.Parse(
                distanceMatches.Aggregate((acc, next) => acc + next)
            );
            var total = 0;
            for (var i = 0; i < recordTime; i++) {
                if (i * (recordTime - i) > recordDistance) {
                    total++;
                }
            }
            return total;
        }
    }

    [GeneratedRegex("Time:((\\s+(\\d+))+)")]
    private static partial Regex TimeRegex();
    [GeneratedRegex("Distance:((\\s+(\\d+))+)")]
    private static partial Regex DistanceRegex();
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}