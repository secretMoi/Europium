namespace Europium.Dtos;

public class ListFilesArguments
{
	public string? Path { get; set; }
	public int Limit { get; set; }
	public FileType FileType { get; set; }
	public bool IsLocal { get; set; }
}