namespace Europium.Dtos;

public class YggTorrentAccount
{
    public decimal Ratio { get; set; }
    public decimal Up { get; set; }
    public string UpUnit { get; set; } = null!;
    public decimal Down { get; set; }
    public string DownUnit { get; set; } = null!;
}