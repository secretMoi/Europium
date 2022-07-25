namespace Europium.Models;

public class AppConfig
{
	public string SshHost { get; set; } = null!;
	public string SshUser { get; set; } = null!;
	public string SshPassword { get; set; } = null!;
	public int SshPort { get; set; }
}