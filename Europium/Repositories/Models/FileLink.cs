namespace Europium.Repositories.Models;

public class FileLink
{
	public int FileLinkId { get; set; }

	public string FileName { get; set; } = null!;

	public string TorrentHash { get; set; } = null!;
	public string TorrentData { get; set; } = null!;

	public int ApiId { get; set; }
	public string ApiData { get; set; } = null!;

	public int TmdbId { get; set; }
	public string TmdbData { get; set; } = null!;

	public int TvdbId { get; set; }
	public string TvdbData { get; set; } = null!;
	
	public DateTime CreatedDate { get; set; }
}