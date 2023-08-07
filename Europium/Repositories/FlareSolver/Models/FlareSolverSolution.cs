namespace Europium.Repositories.FlareSolver.Models;

public class FlareSolverSolution
{
    public string Url { get; set; }
    public int Status { get; set; }
    public List<FlareSolverCookie> Cookies { get; set; }
    public string UserAgent { get; set; }
}