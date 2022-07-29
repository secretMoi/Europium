namespace Europium.Models;

public class ListFilesArguments
{
	public string? Path { get; set; }
	public int Limit { get; set; }
	public FileType FileType { get; set; }
}