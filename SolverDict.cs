namespace ADL.AdventOfCode2023;

static class SolverDict {
    public static Dictionary<int, ISolver> Solvers = new()
    {
        { 1, new CalibrationSolver() },
        { 2, new CubeGameSolver() },
        { 3, new EngineSchematicSolver() },
        { 4, new ScratchCardSolver() },
        { 5, new AlmanacSolver() }
    };
}