using System.Text.RegularExpressions;

namespace AdventOfCode.Solvers._2023;

public partial class DesertMapSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        var instructionsLine = lines[0];
        var nodeRegex = NodeRegex();
        var nodes = new Dictionary<string, (string left, string right)>();
        foreach(var line in lines[2..]) {
            var matchGroups = nodeRegex.Match(line).Groups;
            nodes.Add(matchGroups[1].Value, (left: matchGroups[2].Value, right: matchGroups[3].Value));
        }
        List<(string left, string right)> currentNodes;
        Func<string, bool> isEndpoint;
        if (part == 1) {
            isEndpoint = key => key == "ZZZ";
            currentNodes = nodes.Keys
                .Where(k => k == "AAA")
                .Select(k => nodes[k])
                .ToList();
        } else {
            isEndpoint = key => key.EndsWith('Z');
            var currentNodeKeys = nodes.Keys
                .Where(k => k.EndsWith('A'));
            currentNodes = currentNodeKeys
                .Select(k => nodes[k])
                .ToList();
            var nodesArray = currentNodeKeys.ToArray();
            foreach(var node in nodesArray) {
                Console.WriteLine(node);
            }
        }

        return DetermineStepCount(instructionsLine, currentNodes, nodes, isEndpoint);
    }

    private int DetermineStepCount(
        string instructionsLine,
        List<(string left, string right)> currentNodes,
        Dictionary<string, (string left, string right)> nodes,
        Func<string, bool> isEndpoint
    ) {
        var pathLengths = new int[currentNodes.Count];
        Array.Fill(pathLengths, 0);
        for (var i = 0; i < currentNodes.Count; i++) {
            var currentNode = currentNodes[i];
            while(true) {
                var stepIndex = pathLengths[i] % instructionsLine.Length;
                var currentStep = instructionsLine[stepIndex];
                var nextNodeKey = currentStep switch {
                    'L' => currentNode.left,
                    'R' => currentNode.right,
                    _ => throw new InvalidOperationException($"Invalid step {currentStep}")
                };
                // Console.WriteLine("------");
                // foreach(var key in nextNodeKeys) {
                //     Console.WriteLine(key);
                // }
                pathLengths[i]++;
                if (isEndpoint(nextNodeKey)) {
                    break;
                }
                currentNode = nodes[nextNodeKey];
            }
        }

        return pathLengths.Aggregate(LeastCommonMultiple);
    }

    private int GreatestCommonDivisor(int a, int b) => b == 0 ? a : GreatestCommonDivisor(b, a % b);
    private int LeastCommonMultiple(int a, int b) => Math.Abs(a * b) / GreatestCommonDivisor(a, b);

    [GeneratedRegex("(\\w{3}) = \\((\\w{3}), (\\w{3})\\)")]
    private static partial Regex NodeRegex();
}