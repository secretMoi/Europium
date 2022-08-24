using Europium.Services.Apis.TheMovieDb.Models.Radarr;
using Europium.Services.Apis.TheMovieDb.Models.Sonarr;
using Newtonsoft.Json;

namespace Europium.Services.Apis.TheMovieDb.Models.Tmdb;

public record Media
{
	public int Id { get; set; }
	
	public string Title { get; set; } = null!;
	public string Name { get; set; } = null!;
	public string Overview { get; set; } = null!;
	public string Link { get; set; } = null!;
	public string FileLinkInApi { get; set; } = null!;
	
	[JsonProperty("vote_average")]
	public float VoteAverage { get; set; }
	
	[JsonProperty("original_title")]
	public string OriginalTitle { get; set; } = null!;
	
	[JsonProperty("poster_path")]
	public string PosterPath { get; set; } = null!;
	
	[JsonProperty("backdrop_path")]
	public string BackdropPath { get; set; } = null!;
	
	public List<Season> Seasons { get; set; } = null!;

	public RadarrInformation? RadarrInformation { get; set; }
	public SonarrInformation? SonarrInformation { get; set; }
}