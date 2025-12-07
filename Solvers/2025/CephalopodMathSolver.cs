using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2025;

public partial class CephalopodMathSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1)
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
        }

        if (part == 2)
        {
            var result = BigInteger.Zero;
            var currentOperator = ToOperator(lines[^1][0]);
            BigInteger? currentAnswer = null;

            for (var col = 0; col < lines[0].Length; col++)
            {
                var currentNumberStringBuilder = new StringBuilder();
                foreach (var line in lines[..^1])
                {
                    var currentDigit = line[col];
                    if (currentDigit != ' ')
                    {
                        currentNumberStringBuilder.Append(currentDigit);
                    }
                }

                var currentNumberString = currentNumberStringBuilder.ToString();
                if (string.IsNullOrWhiteSpace(currentNumberString))
                {
                    result += currentAnswer!.Value;
                    currentAnswer = null;
                    continue;
                }

                var currentNumber = BigInteger.Parse(currentNumberString);
                var potentialOperator = lines.Last()[col];
                if (potentialOperator != ' ')
                {
                    currentOperator = ToOperator(potentialOperator);
                }

                currentAnswer = currentAnswer is null
                    ? currentNumber
                    : ApplyOperation(currentAnswer.Value, currentNumber, currentOperator);
            }

            result += currentAnswer!.Value;

            return result;
        }

        return -1;

        Operator ToOperator(char operatorChar) =>
            operatorChar switch
            {
                '+' => Operator.Plus,
                '-' => Operator.Minus,
                '*' => Operator.Times,
                '/' => Operator.Divide,
                _ => throw new InvalidEnumArgumentException($"Invalid operator {operatorChar} encountered")
            };

        BigInteger ApplyOperation(BigInteger value1, BigInteger value2, Operator mathOperator) =>
            mathOperator switch
            {
                Operator.Plus => value1 + value2,
                Operator.Minus => value1 - value2,
                Operator.Times => value1 * value2,
                Operator.Divide => value1 / value2,
                _ => throw new InvalidEnumArgumentException($"Invalid operator {mathOperator} encountered")
            };

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