namespace Europium.Dtos.FlareSolver;

public class SessionsList
{
   public string Status { get; set; }
   public string Message { get; set; }
   public List<string> Sessions { get; set; }
   public long StartTimestamp { get; set; }
   public long EndTimestamp { get; set; }
   public string Version { get; set; }
}