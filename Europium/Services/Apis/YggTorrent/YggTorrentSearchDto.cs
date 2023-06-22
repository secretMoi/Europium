namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentSearchDto
{
    public string Name { get; set; }
    public string PageUrl { get; set; }
    public int TorrentId { get; set; }
    public long Age { get; set; }
    public long Size { get; set; }
    public int Downloaded { get; set; }
    public int Seeders { get; set; }
    public MediaQuality MediaQuality { get; set; }
    public MediaType MediaType { get; set; }
}

public enum MediaQuality
{
    Unknown,
    HD,
    FHD,
    UHD
}

public enum MediaType
{
    Unknown,
    Movie,
    Serie,
    Anime,
}