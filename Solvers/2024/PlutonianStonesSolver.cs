using System.Numerics;

namespace AdventOfCode.Solvers._2024;

public class PlutonianStonesSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var rocks = lines[0]
            .Split(" ")
            .Select(value => new KeyValuePair<BigInteger, BigInteger>(BigInteger.Parse(value), 1))
            .ToDictionary();
        var iterations = part == 1 ? 25 : 75;

        var results = new Dictionary<BigInteger, (BigInteger, BigInteger?)>();
        
        for(var iteration = 0; iteration < iterations; iteration++)
        {
            var copyDict = new Dictionary<BigInteger, BigInteger>();
            foreach (var rock in rocks.Keys)
            {
                if (!results.TryGetValue(rock, out var values))
                {
                    values = ApplyRules(rock);
                    results.Add(rock, values);
                }

                if (copyDict.TryGetValue(values.Item1, out var currentValue))
                {
                    copyDict[values.Item1] = currentValue + rocks[rock];
                }
                else
                {
                    copyDict.Add(values.Item1, rocks[rock]);
                }

                if (values.Item2.HasValue)
                {
                    if (copyDict.TryGetValue(values.Item2.Value, out var currentValue2))
                    {
                        copyDict[values.Item2.Value] = currentValue2 + rocks[rock];
                    }
                    else
                    {
                        copyDict.Add(values.Item2.Value, rocks[rock]);
                    }
                }
            }

            rocks = copyDict;
        }

        return rocks.Values.Aggregate(BigInteger.Add);
    }

    private static (BigInteger, BigInteger?) ApplyRules(BigInteger rock)
    {
        switch (rock)
        {
            case var _ when rock == BigInteger.Zero:
                return (1, null);
            case var _ when ((int)BigInteger.Log10(rock) + 1) % 2 == 0:
                var numDigits = (int)BigInteger.Log10(rock) + 1;
                var digitFactor = BigInteger.Pow(10, numDigits / 2);
                var firstValue = rock / digitFactor;
                var secondValue = rock % (int)digitFactor;
                return (firstValue, secondValue);
            default:
                var newValue = rock * 2024;
                return (newValue, null);
        }
    }
}