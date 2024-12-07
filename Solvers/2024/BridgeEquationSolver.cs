using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2024;

public partial class BridgeEquationSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var equationRegex = EquationRegex();
        BigInteger total = 0;
        if (part == 1)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var line in lines)
            {
                var match = equationRegex.Match(line);
                BigInteger testValue;
                try
                {
                    testValue = BigInteger.Parse(match.Groups[1].Value);
                }
                catch (OverflowException)
                {
                    testValue = -1;
                }

                var operands = match.Groups[2].Value.Split(' ').Select(BigInteger.Parse).ToArray();
                var resultCandidates = ComputeResultCandidates(operands);
                if (resultCandidates.Any(result => result == testValue))
                {
                    total += testValue;
                }
            }
        }

        return total;
    }

    private static BigInteger[] ComputeResultCandidates(BigInteger[] operands)
    {
        if (operands.Length == 2)
        {
            return
            [
                operands[0] + operands[1],
                operands[0] * operands[1]
            ];
        }

        return ComputeResultCandidates(operands[..^1]).SelectMany(result => new[]
        {
            operands[^1] + result,
            operands[^1] * result
        }).ToArray();
    }

    [GeneratedRegex(@"^(\d+): ((\d+ ?)+)")]
    private static partial Regex EquationRegex();
}