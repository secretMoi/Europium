using Newtonsoft.Json;

namespace Europium.Services.Apis.TheMovieDb.Models.Tmdb;

public class SerieIdLinkToOtherApi
{
	[JsonProperty("imdb_id")]
	public string ImdbId { get; set; } = null!;

	[JsonProperty("freebase_mid")]
	public string FreebaseMid { get; set; } = null!;

	[JsonProperty("freebase_id")]
	public string FreebaseId { get; set; } = null!;

	[JsonProperty("tvdb_id")]
	public int TvdbId { get; set; }

	[JsonProperty("tvrage_id")]
	public int? TvrageId { get; set; }

	[JsonProperty("facebook_id")]
	public string FacebookId { get; set; } = null!;

	[JsonProperty("instagram_id")]
	public string InstagramId { get; set; } = null!;

	[JsonProperty("twitter_id")]
	public string TwitterId { get; set; } = null!;

	[JsonProperty("id")]
	public int Id { get; set; }
}