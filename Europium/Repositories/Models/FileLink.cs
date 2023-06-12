namespace Europium.Repositories.Models;

public class FileLink
{
	public int FileLinkId { get; set; }

	public string FileName { get; set; }

	public string TorrentHash { get; set; }
	public string TorrentData { get; set; }

	public int ApiId { get; set; }
	public string ApiData { get; set; }

	public int TmdbId { get; set; }
	public string TmdbData { get; set; }

	public int TvdbId { get; set; }
	public string TvdbData { get; set; }
	
	public DateTime CreatedDate { get; set; }
}