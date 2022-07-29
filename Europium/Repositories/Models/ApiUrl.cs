using Newtonsoft.Json;

namespace Europium.Repositories.Models;

public class ApiUrl
{
	public int ApiUrlId { get; set; }
	
	public string Url { get; set; }
	
	public int ApiToMonitorId { get; set; }
	
	[JsonIgnore]
	public ApiToMonitor ApiToMonitor { get; set; }
}