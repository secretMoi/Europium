using Europium.Repositories.Models;

namespace Europium.Dtos;

public class MonitoredApiDto
{
	public int ApiToMonitorId { get; set; }
	public string? Name { get; set; }
	public string Code { get; set; }
	public string? Logo { get; set; }
	public string? Url { get; set; }
	public bool? State { get; set; }

	public List<ApiUrl>? ApiUrls { get; set; }
}