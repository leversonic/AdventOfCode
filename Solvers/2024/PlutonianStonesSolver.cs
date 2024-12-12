using System.Numerics;

namespace AdventOfCode.Solvers._2024;

public class PlutonianStonesSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var rocks = lines[0].Split(" ").Select(BigInteger.Parse).ToList();
        var iterations = 0;
        if (part == 1)
        {
            iterations = 25;
        }
        
        for(var iteration = 0; iteration < iterations; iteration++)
        {
            var rockIndex = 0;
            while (rockIndex < rocks.Count)
            {
                var rock = rocks[rockIndex];
                switch (rock)
                {
                    case var _ when rock == BigInteger.Zero:
                        rocks[rockIndex] = 1;
                        break;
                    case var _ when rock.ToString().Length % 2 == 0:
                        var numDigits = rock.ToString().Length;
                        var digitFactor = BigInteger.Pow(10, numDigits / 2);
                        rocks[rockIndex] = rock / digitFactor;
                        rocks.Insert(rockIndex + 1, rock % (int)digitFactor);
                        rockIndex++;
                        break;
                    default:
                        rocks[rockIndex] *= 2024;
                        break;
                }
                rockIndex++;
            }
        }

        return rocks.Count;
    }
}