using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

public partial class EngineSchematicSolver : ISolver
{
    public int Solve(string[] lines, int part)
    {
        if (part == 1) {
            var numericRegex = NumberRegex();

            return lines
                .Select((lineText, lineIndex) => (lineText, lineIndex))
                .Sum(tuple => {
                    return numericRegex.Matches(tuple.lineText)
                        .Sum(match => IsPartNumber(match, tuple.lineIndex, lines) ? int.Parse(match.Value) : 0);
                });
        } else {
            throw new NotImplementedException("Part 2 not yet implemented");
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