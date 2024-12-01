using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

public partial class AlmanacSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1) {
            var seeds = SeedsRegex()
                .Match(lines[0])
                .Groups[1]
                .Value
                .Split(' ')
                .Select(BigInteger.Parse);

            AlmanacTable? currentTable = null;
            List<AlmanacTable> tables = [];
            foreach(var line in lines[2..]) {
                var titleMatch = TableHeaderRegex().Match(line);
                var mapMatch = MapRegex().Match(line);
                if (titleMatch.Success) {
                    if (currentTable != null) {
                        tables.Add(currentTable);
                    }
                    currentTable = new AlmanacTable{
                        InputType = titleMatch.Groups[1].Value,
                        OutputType = titleMatch.Groups[2].Value
                    };
                    continue;
                }

                if (mapMatch.Success) {
                    currentTable!.AddMap(
                        BigInteger.Parse(mapMatch.Groups[1].Value),
                        BigInteger.Parse(mapMatch.Groups[2].Value),
                        BigInteger.Parse(mapMatch.Groups[3].Value)
                    );
                    continue;
                }
            }

            if (currentTable != null) {
                tables.Add(currentTable!);
            }

            return seeds
                .Select(seed => {
                    var currentKey = "seed";
                    var currentValue = seed;
                    while(true) {
                        var table = tables.First(t => t.InputType == currentKey);
                        currentValue = table.Convert(currentValue);
                        if (table.OutputType == "location") {
                            return currentValue;
                        }
                        currentKey = table.OutputType;
                    }
                })
                .Min();
        } else {
            throw new NotImplementedException();
        }
    }

    public class AlmanacTable
    {
        private readonly List<(BigInteger sourceRangeStart, BigInteger rangeLength, BigInteger destinationOffset)> maps = [];

        public required string InputType { get; init; }
        public required string OutputType { get; init; }

        public void AddMap(BigInteger destinationRangeStart, BigInteger sourceRangeStart, BigInteger rangeLength) {
            var destinationOffset = destinationRangeStart - sourceRangeStart;
            maps.Add((sourceRangeStart, rangeLength, destinationOffset));
        }

        public BigInteger Convert(BigInteger sourceValue) {
            var mapForValue = maps
                .FirstOrDefault(map =>
                    map.sourceRangeStart < sourceValue
                        && sourceValue <= map.sourceRangeStart + map.rangeLength
                );
            if (mapForValue == default) {
                return sourceValue;
            }

            return sourceValue + mapForValue.destinationOffset;
        }
    }

    [GeneratedRegex("(\\d+) (\\d+) (\\d+)")]
    private static partial Regex MapRegex();

    [GeneratedRegex("seeds: ((\\d+ ?)+)")]
    private static partial Regex SeedsRegex();
    [GeneratedRegex("(\\w+)-to-(\\w+) map:")]
    private static partial Regex TableHeaderRegex();
}