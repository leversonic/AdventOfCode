using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

public partial class EngineSchematicSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
            var numericRegex = NumberRegex();
        if (part == 1) {
            return lines
                .Select((lineText, lineIndex) => (lineText, lineIndex))
                .Sum(tuple => {
                    return numericRegex.Matches(tuple.lineText)
                        .Sum(match => IsPartNumber(match, tuple.lineIndex, lines) ? int.Parse(match.Value) : 0);
                });
        } else {
            var numberRegions = lines
                .Select((line, lineIndex) =>
                    (
                        lineIndex,
                        partNumbers: numericRegex.Matches(line)
                            .Select(m => (start: m.Index, end: m.Index + m.Length - 1))
                    ))
                .Where(tuple => tuple.partNumbers.Any());
            var gearLocations = lines
                .Select((line, lineIndex) => {
                    return line
                        .Select((character, characterIndex) => (character, characterIndex))
                        .Where(tuple => tuple.character == '*')
                        .Select(tuple => (x: tuple.characterIndex, y: lineIndex));
                }).Aggregate(new List<(int x, int y)>(), (acc, current) => {
                    foreach (var c in current) {
                        acc.Add(c);
                    }
                    return acc;
                });

            return gearLocations.Sum(gearLocation => {
                var adjacentNumericGroups = numberRegions.Select(region => {
                    if (Math.Abs(gearLocation.y - region.lineIndex) == 1) {
                        return region.partNumbers.Where(partNumber =>
                            gearLocation.x >= partNumber.start - 1 && gearLocation.x <= partNumber.end + 1
                        ).Select(tuple => (tuple.start, tuple.end, row: region.lineIndex));
                    } else if (gearLocation.y == region.lineIndex) {
                        return region.partNumbers.Where(partNumber =>
                            gearLocation.x == partNumber.start - 1 || gearLocation.x == partNumber.end + 1
                        ).Select(tuple => (tuple.start, tuple.end, row: region.lineIndex));
                    }
                    return new List<(int start, int end, int row)>();
                }).Aggregate(new List<(int start, int end, int row)>(), (acc, current) => {
                    foreach(var c in current) {
                        acc.Add(c);
                    }
                    return acc;
                });
                if (adjacentNumericGroups.Count == 2) {
                    var (start1, end1, row1) = adjacentNumericGroups[0];
                    var item1 = lines[row1][start1..(end1 + 1)];
                    var (start2, end2, row2) = adjacentNumericGroups[1];
                    var item2 = lines[row2][start2..(end2 + 1)];
                    return int.Parse(item1) * int.Parse(item2);
                }
                return 0;
            });
        }
    }

    private bool IsPartNumber(Match match, int lineIndex, string[] lines) {
        var coordinatesToCheck = new List<(int x, int y)>
        {
            (x: match.Index - 1, y: lineIndex),
            (x: match.Index + match.Length, y: lineIndex)
        };
        for(var i = match.Index - 1; i <= match.Index + match.Length; i++) {
            coordinatesToCheck.Add((x: i, y: lineIndex - 1));
            coordinatesToCheck.Add((x: i, y: lineIndex + 1));
        }

        return coordinatesToCheck
            .Where(c => c.x >= 0 && c.x < lines[lineIndex].Length && c.y >= 0 && c.y < lines.Length)
            .Any(c => SymbolRegex().IsMatch(lines[c.y][c.x].ToString()));
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();

    [GeneratedRegex("[^\\d\\.]")]
    private static partial Regex SymbolRegex();
}