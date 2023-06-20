namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentSearchDto
{
    public string Name { get; set; }
    public string PageUrl { get; set; }
    public string TorrentUrl { get; set; }
    public string Age { get; set; }
    public string Size { get; set; }
    public string Downloaded { get; set; }
    public string Seeders { get; set; }
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
}