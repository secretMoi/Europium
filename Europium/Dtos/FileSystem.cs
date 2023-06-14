namespace Europium.Dtos;

public class FileSystem
{
	public long? Size { get; set; }
	public long? Used { get; set; }
	public long? Available { get; set; }
	public string? PercentageUsed { get; set; }
	public string? Volume { get; set; }
	public bool IsLocal { get; set; }
}