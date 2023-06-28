namespace Europium.Dtos.Plex;

public class PlexDuplicateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public long TotalSize { get; set; }

    public List<PlexMediaDto> PlexMedias { get; set; }
}