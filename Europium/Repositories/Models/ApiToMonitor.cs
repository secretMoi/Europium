namespace Europium.Repositories.Models;

public class ApiToMonitor
{
	public int ApiToMonitorId { get; set; }
	public string? Name { get; set; }
	public string Code { get; set; } = null!;
	public string? Logo { get; set; }
	public string? Url { get; set; }
	public string? ApiKey { get; set; }
	public string? UserName { get; set; }
	public string? Password { get; set; }
	
	

	public virtual List<ApiUrl> ApiUrls { get; set; } = null!;
}