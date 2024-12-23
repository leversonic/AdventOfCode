using AdventOfCode.Solvers;
using AdventOfCode.Solvers._2023;
using AdventOfCode.Solvers._2024;

namespace AdventOfCode;

public static class SolverDict
{
    public static readonly Dictionary<int, Dictionary<int, ISolver>> Solvers = new()
    {
        {
            2023,
            new()
            {
                { 1, new CalibrationSolver() },
                { 2, new CubeGameSolver() },
                { 3, new EngineSchematicSolver() },
                { 4, new ScratchCardSolver() },
                { 5, new AlmanacSolver() },
                { 6, new BoatRaceSolver() },
                { 7, new CamelPokerSolver() },
                { 8, new DesertMapSolver() }
            }
        },
        {
            2024,
            new()
            {
                { 1, new ListDistanceSolver() },
                { 2, new NuclearLevelSolver() },
                { 3, new MultiplicationProgramSolver() },
                { 4, new XmasCrosswordSolver() },
                { 5, new SafetyManualPageSolver() },
                { 6, new GuardPathSolver() },
                { 7, new BridgeEquationSolver() },
                { 8, new AntennaAntinodeSolver() },
                { 9, new HardDriveDefragmentationSolver() },
                { 10, new HikingTrailSolver() },
                { 11, new PlutonianStonesSolver() },
                { 12, new GardenRegionsSolver() },
                { 13, new ClawMachineSolver() },
                { 14, new BathroomRobotSolver() },
                { 15, new WarehouseRobotSolver() }
            }
        }
    };
}