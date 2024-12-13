namespace AdventOfCode.Solvers._2024;

public class GardenRegionsSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var total = 0;

        var regions = new List<List<(int x, int y)>>();
        for (var x = 0; x < lines[0].Length; x++)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                if (!regions.Any(r => r.Contains((x, y))))
                {
                    var region = new List<(int x, int y)> { (x, y) };
                    PopulateRegion(region, lines, x, y);
                    regions.Add(region);
                }
            }
        }

        foreach (var region in regions)
        {
            var area = region.Count;
            if (part == 1)
            {
                var perimeter = 0;
                foreach (var (x, y) in region)
                {
                    if (!region.Contains((x - 1, y)))
                    {
                        perimeter++;
                    }

                    if (!region.Contains((x + 1, y)))
                    {
                        perimeter++;
                    }

                    if (!region.Contains((x, y - 1)))
                    {
                        perimeter++;
                    }

                    if (!region.Contains((x, y + 1)))
                    {
                        perimeter++;
                    }
                }

                total += perimeter * area;
            }
            else
            {
                var cornersCount = 0;
                foreach (var (x, y) in region)
                {
                    // Regular corners
                    if (!region.Contains((x - 1, y - 1)))
                    {
                        if (region.Contains((x - 1, y)) ^ !region.Contains((x, y - 1)))
                        {
                            cornersCount++;
                        }
                    }

                    if (!region.Contains((x - 1, y + 1)))
                    {
                        if (region.Contains((x - 1, y)) ^ !region.Contains((x, y + 1)))
                        {
                            cornersCount++;
                        }
                    }

                    if (!region.Contains((x + 1, y - 1)))
                    {
                        if (region.Contains((x + 1, y)) ^ !region.Contains((x, y - 1)))
                        {
                            cornersCount++;
                        }
                    }

                    if (!region.Contains((x + 1, y + 1)))
                    {
                        if (region.Contains((x + 1, y)) ^ !region.Contains((x, y + 1)))
                        {
                            cornersCount++;
                        }
                    }

                    // Literal corner cases
                    if (region.Contains((x + 1, y + 1)) && !region.Contains((x, y + 1)) && !region.Contains((x + 1, y)))
                    {
                        cornersCount++;
                    }

                    if (region.Contains((x + 1, y - 1)) && !region.Contains((x, y - 1)) && !region.Contains((x + 1, y)))
                    {
                        cornersCount++;
                    }
                    
                    if (region.Contains((x - 1, y + 1)) && !region.Contains((x, y + 1)) && !region.Contains((x - 1, y)))
                    {
                        cornersCount++;
                    }
                    
                    if (region.Contains((x - 1, y - 1)) && !region.Contains((x, y - 1)) && !region.Contains((x - 1, y)))
                    {
                        cornersCount++;
                    }
                }

                total += cornersCount * area;
            }
        }

        return total;
    }

    private static void PopulateRegion(List<(int x, int y)> currentValues, string[] map, int x, int y)
    {
        var currentLetter = map[y][x];
        if (x > 0)
        {
            if (map[y][x - 1] == currentLetter && !currentValues.Contains((x - 1, y)))
            {
                currentValues.Add((x - 1, y));
                PopulateRegion(currentValues, map, x - 1, y);
            }
        }

        if (x < map[0].Length - 1)
        {
            if (map[y][x + 1] == currentLetter && !currentValues.Contains((x + 1, y)))
            {
                currentValues.Add((x + 1, y));
                PopulateRegion(currentValues, map, x + 1, y);
            }
        }

        if (y > 0)
        {
            if (map[y - 1][x] == currentLetter && !currentValues.Contains((x, y - 1)))
            {
                currentValues.Add((x, y - 1));
                PopulateRegion(currentValues, map, x, y - 1);
            }
        }

        if (y < map.Length - 1)
        {
            if (map[y + 1][x] == currentLetter && !currentValues.Contains((x, y + 1)))
            {
                currentValues.Add((x, y + 1));
                PopulateRegion(currentValues, map, x, y + 1);
            }
        }
    }
}