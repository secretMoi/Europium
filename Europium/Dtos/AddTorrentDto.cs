using Europium.Services.Apis.YggTorrent;

namespace Europium.Dtos;

public class AddTorrentDto
{
    public int TorrentId { get; set; }
    public MediaType MediaType { get; set; }
}