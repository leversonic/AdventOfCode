using System.Text.RegularExpressions;

namespace ADL.AdventOfCode2023;

public partial class DesertMapSolver : ISolver
{
    public object Solve(string[] lines, int part)
    {
        if (part == 1) {
            var instructionsLine = lines[0];
            var nodeRegex = NodeRegex();
            var nodes = new Dictionary<string, (string left, string right)>();
            foreach(var line in lines[2..]) {
                var matchGroups = nodeRegex.Match(line).Groups;
                nodes.Add(matchGroups[1].Value, (left: matchGroups[2].Value, right: matchGroups[3].Value));
            }
            var stepCount = 0;
            var stepIndex = 0;
            var currentNodeKey = "AAA";
            var currentNode = nodes[currentNodeKey];
            while(true) {
                var currentStep = instructionsLine[stepIndex];
                var nextNodeKey = currentStep switch {
                    'L' => currentNode.left,
                    'R' => currentNode.right,
                    _ => throw new InvalidOperationException($"Invalid step {currentStep}")
                };
                stepCount++;
                if (nextNodeKey == "ZZZ") {
                    break;
                }
                currentNode = nodes[nextNodeKey];
                if (stepIndex == instructionsLine.Length - 1) {
                    stepIndex = 0;
                } else {
                    stepIndex++;
                }
            }

            return stepCount;
        } else {
            throw new NotImplementedException();
        }
    }

    [GeneratedRegex("(\\w{3}) = \\((\\w{3}), (\\w{3})\\)")]
    private static partial Regex NodeRegex();
}