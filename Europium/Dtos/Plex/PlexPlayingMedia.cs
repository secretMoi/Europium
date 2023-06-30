namespace Europium.Dtos.Plex;

public class PlexPlayingMedia
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int RemoteBitrate { get; set; }
    public bool IsPlaying { get; set; }
    public bool IsRemote { get; set; }
    public int Progress { get; set; }
    public int Duration { get; set; }
    public int Year { get; set; }
    public int ThumbnailId { get; set; }
    
    public string UserName { get; set; }
    
    public bool IsVideoTranscoding { get; set; }
    public string RemoteResolution { get; set; }
    public string VideoCodec { get; set; }
    
    public bool IsAudioTranscoding { get; set; }
    public string AudioTitle { get; set; }
}