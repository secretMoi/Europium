using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Europium.Repositories.Models;

public class ApiUrl
{
	public int ApiUrlId { get; set; }
	
	public string Url { get; set; }
	
	public int ApiToMonitorId { get; set; }
	
	[NotMapped]
	public bool? State { get; set; }
	
	[JsonIgnore]
	public ApiToMonitor ApiToMonitor { get; set; }
}