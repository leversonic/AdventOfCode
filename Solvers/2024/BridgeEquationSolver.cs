using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class BridgeEquationSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var equationRegex = EquationRegex();
        BigInteger total = 0;
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var line in lines)
        {
            var match = equationRegex.Match(line);

            var testValue = BigInteger.Parse(match.Groups[1].Value);

            var operands = match.Groups[2].Value.Split(' ').Select(BigInteger.Parse).ToArray();
            var resultCandidates = ComputeResultCandidates(operands, part == 2);
            if (resultCandidates.Any(result => result == testValue))
            {
                total += testValue;
            }
        }

        return total;
    }

    private static BigInteger[] ComputeResultCandidates(BigInteger[] operands, bool supportConcatenation)
    {
        if (operands.Length == 2)
        {
            var result = new[]
            {
                operands[0] + operands[1],
                operands[0] * operands[1]
            };
            if (supportConcatenation)
            {
                result = result.Concat([BigInteger.Parse($"{operands[0]}{operands[1]}")]).ToArray();
            }
            return result;
        }

        return ComputeResultCandidates(operands[..^1], supportConcatenation)
            .SelectMany(result =>
            {
                var returnValue = new[]
                {
                    result + operands[^1],
                    result * operands[^1]
                };
                if (supportConcatenation)
                {
                    returnValue = returnValue.Concat([BigInteger.Parse($"{result}{operands[^1]}")]).ToArray();
                }
                return returnValue;
            }).ToArray();
    }

    [GeneratedRegex(@"^(\d+): ((\d+ ?)+)")]
    private static partial Regex EquationRegex();
}