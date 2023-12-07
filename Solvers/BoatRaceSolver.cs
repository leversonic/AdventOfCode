using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

public partial class BoatRaceSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1) {
            var timesGroup = TimeRegex().Match(lines[0]).Groups[1].Value.Trim();
            var timeMatches = NumberRegex().Matches(timesGroup).Select(m => m.Value).ToArray();
            var distanceGroup = DistanceRegex().Match(lines[1]).Groups[1].Value.Trim();
            var distanceMatches = NumberRegex().Matches(distanceGroup).Select(m => m.Value).ToArray();
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
            throw new NotImplementedException();
        }
    }

    [GeneratedRegex("Time:((\\s+(\\d+))+)")]
    private static partial Regex TimeRegex();
    [GeneratedRegex("Distance:((\\s+(\\d+))+)")]
    private static partial Regex DistanceRegex();
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}