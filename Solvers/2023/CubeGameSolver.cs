using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

partial class CubeGameSolver : ISolver
{
    private const int RedCount = 12;
    private const int GreenCount = 13;
    private const int BlueCount = 14;

    public object Solve(string[] lines, int part)
    {
        var games = ParseGamesFromInput(lines);
        if (part == 1) {
            return games.Sum(g => {
                if (g.Sets.Any(s => s.Red > RedCount || s.Green > GreenCount || s.Blue > BlueCount)) {
                    return 0;
                }
                return g.Id;
            });
        } else {
            return games.Sum(g => {
                var minRed = g.Sets
                    .Select(g => g.Red)
                    .Max();
                var minGreen = g.Sets
                    .Select(g => g.Green)
                    .Max();
                var minBlue = g.Sets
                    .Select(g => g.Blue)
                    .Max();
                return minRed * minGreen * minBlue;
            });
        }
    }

    private IEnumerable<CubeGame> ParseGamesFromInput(string[] lines) {
        return lines
            .Select(line =>
            {
                var setStrings = line.Split(';');
                var redRegex = RedRegex();
                var greenRegex = GreenRegex();
                var blueRegex = BlueRegex();
                var idRegex = IdRegex();
                return new CubeGame
                {
                    Id = int.Parse(idRegex.Match(line).Groups[1].Value),
                    Sets = setStrings
                        .Select(str =>
                        {
                            var set = new CubeSet();
                            var redMatch = redRegex.Match(str);
                            if (redMatch.Success)
                            {
                                set.Red = int.Parse(redMatch.Groups[1].Value);
                            }
                            var greenMatch = greenRegex.Match(str);
                            if (greenMatch.Success)
                            {
                                set.Green = int.Parse(greenMatch.Groups[1].Value);
                            }
                            var blueMatch = blueRegex.Match(str);
                            if (blueMatch.Success)
                            {
                                set.Blue = int.Parse(blueMatch.Groups[1].Value);
                            }

                            return set;
                        })
                };
            });
    }

    public class CubeGame {
        public int Id { get; init; }
        public IEnumerable<CubeSet> Sets { get; init; } = new List<CubeSet>();
    }

    public class CubeSet {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }

    [GeneratedRegex("(\\d+) red")]
    private static partial Regex RedRegex();
    [GeneratedRegex("(\\d+) green")]
    private static partial Regex GreenRegex();
    [GeneratedRegex("(\\d+) blue")]
    private static partial Regex BlueRegex();
    [GeneratedRegex("Game (\\d+)")]
    private static partial Regex IdRegex();
}