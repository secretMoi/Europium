namespace Europium.Models;

public class AppConfig
{
	public string SshHost { get; set; } = null!;
	public string SshUser { get; set; } = null!;
	public string SshPassword { get; set; } = null!;
	public int SshPort { get; set; }
	public string EuropiumDatabase { get; set; } = null!;
	public string ApiToMonitorImagePath { get; set; } = null!;
	public string ServerUrl { get; set; } = null!;
	public List<string> AllowedUrls { get; set; } = null!;
	public TheMovieDbConfig TheMovieDb { get; set; }
}