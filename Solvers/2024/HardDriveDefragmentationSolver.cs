using System.Numerics;

namespace AdventOfCode.Solvers._2024;

public class HardDriveDefragmentationSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var hardDrive = new List<int?>();
        for (var i = 0; i < lines[0].Length; i++)
        {
            var fileBlockLength = int.Parse(lines[0][i].ToString());
            for (var j = 0; j < fileBlockLength; j++)
            {
                if (i % 2 == 0)
                {
                    hardDrive.Add(i / 2);
                }
                else
                {
                    hardDrive.Add(null);
                }
            }
        }

        var previousNullIndex = 0;
        var previousLastValueIndex = hardDrive.Count - 1;
        while (true)
        {
            var firstNullIndex = hardDrive.FindIndex(previousNullIndex, v => !v.HasValue);
            previousNullIndex = firstNullIndex;
            var lastValueIndex = hardDrive.FindLastIndex(previousLastValueIndex, v => v.HasValue);
            previousLastValueIndex = lastValueIndex;
            if (firstNullIndex > lastValueIndex)
            {
                break;
            }
            hardDrive[firstNullIndex] = hardDrive[lastValueIndex];
            hardDrive[lastValueIndex] = null;
        }

        BigInteger total = 0;
        if (part == 1)
        {
            var valueSubset = hardDrive[..previousNullIndex];
            for (var i = 0; i < valueSubset.Count; i++)
            {
                total += i * valueSubset[i]!.Value;

            }
        }

        return total;
    }
}