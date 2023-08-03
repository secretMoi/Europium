using Newtonsoft.Json;

namespace Europium.Dtos.FlareSolver;

public class FlareSolverCommand
{
    [JsonProperty("cmd")]
    public string Command { get; set; }
    
    [JsonProperty("url")]
    public string Url { get; set; }
    
    [JsonProperty("session")]
    public string Session { get; set; }
    
    [JsonProperty("returnOnlyCookies")]
    public bool ReturnOnlyCookies { get; set; }
    
    [JsonProperty("maxTimeout")]
    public int MaxTimeout { get; set; }
}