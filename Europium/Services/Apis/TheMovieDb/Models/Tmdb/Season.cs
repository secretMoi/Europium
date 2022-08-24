using Newtonsoft.Json;

namespace Europium.Services.Apis.TheMovieDb.Models.Tmdb;

public class Season
{
	[JsonProperty("air_date")]
	public string? AirDate { get; set; }

	[JsonProperty("episode_count")]
	public int EpisodeCount { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }

	[JsonProperty("overview")]
	public string? Overview { get; set; }

	[JsonProperty("poster_path")]
	public string? PosterPath { get; set; }

	[JsonProperty("season_number")]
	public int SeasonNumber { get; set; }
}