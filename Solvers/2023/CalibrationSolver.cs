using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

partial class CalibrationSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1) {
            var regex = CalibrationPart1Regex();

            return lines.Sum(line => {
                var matches = regex.Matches(line);
                return int.Parse(matches.First().Value + matches.Last().Value);
            });
        } else {
            var regex = CalibrationPart2Regex();

            return lines.Sum(line => {
                var firstMatch = regex.Match(line);
                var lastMatch = regex.Match(line);
                while(true) {
                    var nextMatch = regex.Match(line, lastMatch.Index + 1);
                    if (nextMatch.Success) {
                        lastMatch = nextMatch;
                    } else {
                        break;
                    }
                }

                return int.Parse(ConvertToDecimalString(firstMatch.Value) + ConvertToDecimalString(lastMatch.Value));
            });
        }
    }

    private string ConvertToDecimalString(string inputString) => inputString switch
    {
        "1" or "one" => "1",
        "2" or "two" => "2",
        "3" or "three" => "3",
        "4" or "four" => "4",
        "5" or "five" => "5",
        "6" or "six" => "6",
        "7" or "seven" => "7",
        "8" or "eight" => "8",
        "9" or "nine" => "9",
        _ => throw new ArgumentException($"Invalid decimal detected: {inputString}"),
    };

    [GeneratedRegex("\\d")]
    private static partial Regex CalibrationPart1Regex();

    [GeneratedRegex("([1-9]|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex CalibrationPart2Regex();
}