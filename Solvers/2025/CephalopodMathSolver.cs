using System.ComponentModel;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2025;

public partial class CephalopodMathSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var operators = SplitLine(lines.Last()).Select(operatorString => operatorString switch
        {
            "+" => Operator.Plus,
            "-" => Operator.Minus,
            "*" => Operator.Times,
            "/" => Operator.Divide,
            _ => throw new InvalidEnumArgumentException($"Invalid operator {operatorString} encountered")
        }).ToArray();
        var problemsGrid = lines[..^1].Select(line => SplitLine(line).Select(BigInteger.Parse).ToArray()).ToArray();
        var solutions = problemsGrid.First();
        foreach (var problemRow in problemsGrid[1..])
        {
            for (var problemIndex = 0; problemIndex < operators.Length; problemIndex++)
            {
                switch (operators[problemIndex])
                {
                    case Operator.Plus:
                        solutions[problemIndex] += problemRow[problemIndex];
                        break;
                    case Operator.Minus:
                        solutions[problemIndex] -= problemRow[problemIndex];
                        break;
                    case Operator.Times:
                        solutions[problemIndex] *= problemRow[problemIndex];
                        break;
                    case Operator.Divide:
                        solutions[problemIndex] /= problemRow[problemIndex];
                        break;
                    default:
                        throw new InvalidEnumArgumentException("Invalid operator encountered");
                }
            }
        }

        return solutions.Aggregate((acc, next) => acc + next);

        IEnumerable<string> SplitLine(string line) => WhiteSpaceRegex()
            .Replace(line, ",")
            .Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s));
    }

    private enum Operator
    {
        Plus,
        Minus,
        Times,
        Divide,
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaceRegex();
}