namespace Europium.Dtos.Plex;

public class PlexMediaDto
{
    public int Id { get; set; }
    public int Bitrate { get; set; }
    public string Resolution { get; set; }
    public string FilePath { get; set; }
    public string VideoCodec { get; set; }
    public long Size { get; set; }
}