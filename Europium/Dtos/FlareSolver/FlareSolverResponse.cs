namespace Europium.Dtos.FlareSolver;

public class FlareSolverResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public FlareSolverSolution Solution { get; set; }
    public long StartTimestamp { get; set; }
    public long EndTimestamp { get; set; }
    public string Version { get; set; }
}