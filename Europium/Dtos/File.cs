namespace Europium.Dtos;

public class File
{
	public string? Path { get; set; }
	public long Size { get; set; }
	
	public File(string? path, long size)
	{
		Path = path;
		Size = size;
	}

	public File()
	{
		
	}
}