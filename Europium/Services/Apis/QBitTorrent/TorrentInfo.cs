using Newtonsoft.Json;

namespace Europium.Services.Apis.QBitTorrent;

public class TorrentInfo
{
	public string Name { get; set; }
	public string Hash { get; set; }
	public string Category { get; set; }
	public string State { get; set; }
	public long Size { get; set; }
	public float Progress { get; set; }
	public int Eta { get; set; }
	[JsonProperty("completion_on")]
	public int CompletionOn { get; set; }
	public long Completed { get; set; }
	public long Downloaded { get; set; }
	[JsonProperty("dlspeed")]
	public long DownloadSpeed { get; set; }
	[JsonProperty("total_size")]
	public long TotalSize { get; set; }
	[JsonProperty("save_path")]
	public string SavePath { get; set; }
}