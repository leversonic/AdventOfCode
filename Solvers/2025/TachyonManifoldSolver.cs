namespace AdventOfCode.Solvers._2025;

public class TachyonManifoldSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var charLines = lines.Select(line => line.ToCharArray()).ToArray();
        var currentBeamIndices = new List<int>();
        var splitCount = 0;
        foreach (var line in charLines)
        {
            foreach (var beamIndex in currentBeamIndices)
            {
                if (line[beamIndex] == '^')
                {
                    if (beamIndex > 0)
                    {
                        line[beamIndex - 1] = '|';
                    }

                    if (beamIndex < charLines.Length - 1)
                    {
                        line[beamIndex + 1] = '|';
                    }

                    splitCount++;
                }
                else
                {
                    line[beamIndex] = '|';
                }
            }

            currentBeamIndices = line.Select((c, i) => (Char: c, Index: i))
                .Where(tuple => tuple.Char is '|' or 'S').Select(tuple => tuple.Index).ToList();
        }

        return splitCount;
    }
}