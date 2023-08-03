namespace Europium.Dtos.FlareSolver;

public class FlareSolverCookie
{
    public string Domain { get; set; }
    public long Expiry { get; set; }
    public bool HttpOnly { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string SameSite { get; set; }
    public bool Secure { get; set; }
    public string Value { get; set; }
}