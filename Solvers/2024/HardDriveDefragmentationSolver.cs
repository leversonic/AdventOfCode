using System.Numerics;

namespace AdventOfCode.Solvers._2024;

public class HardDriveDefragmentationSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var hardDrive = new List<int?>();
        var fileBlockInfo = new List<(int? id, int length)>();
        for (var i = 0; i < lines[0].Length; i++)
        {
            var fileBlockLength = int.Parse(lines[0][i].ToString());
            if (i % 2 == 0)
            {
                fileBlockInfo.Add((i / 2, fileBlockLength));
                for (var j = 0; j < fileBlockLength; j++)
                {
                    hardDrive.Add(i / 2);
                }
            }
            else
            {
                fileBlockInfo.Add((null, fileBlockLength));
                for (var j = 0; j < fileBlockLength; j++)
                {
                    hardDrive.Add(null);
                }
            }
        }

        BigInteger total = 0;
        if (part == 1)
        {
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

            var valueSubset = hardDrive[..previousNullIndex];
            for (var i = 0; i < valueSubset.Count; i++)
            {
                total += i * valueSubset[i]!.Value;

            }
        }
        else
        {
            var reverseSortedValues = fileBlockInfo
                .Where(i => i.id != null)
                .OrderByDescending(i => i.id!.Value);
            foreach (var fileBlock in reverseSortedValues)
            {
                var currentFileBlockIndex = fileBlockInfo.IndexOf(fileBlock);
                var indexOfDestinationBlock =
                    fileBlockInfo.FindIndex(i => !i.id.HasValue && i.length >= fileBlock.length);
                if (indexOfDestinationBlock != -1 && indexOfDestinationBlock < currentFileBlockIndex)
                {
                    fileBlockInfo.RemoveAt(currentFileBlockIndex);
                    fileBlockInfo.Insert(currentFileBlockIndex, (id: null, length: fileBlock.length));
                    fileBlockInfo.Insert(indexOfDestinationBlock, fileBlock);
                    var nullFileBlock = fileBlockInfo[indexOfDestinationBlock + 1];
                    if (nullFileBlock.length == fileBlock.length)
                    {
                        fileBlockInfo.RemoveAt(indexOfDestinationBlock + 1);
                    }
                    else
                    {
                        fileBlockInfo[indexOfDestinationBlock + 1] = (null,
                            nullFileBlock.length - fileBlock.length);
                    }
                }
            }

            for(var i = 0; i < fileBlockInfo.Count; i++)
            {
                var (id, length) = fileBlockInfo[i];
                var startIndex = fileBlockInfo[..i].Sum(fileBlock => fileBlock.length);
                for (var j = 0; j < length; j++)
                {
                    total += (id ?? 0) * (j + startIndex);
                }
            }
        }

        return total;
    }
}