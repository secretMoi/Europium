﻿using Newtonsoft.Json;

namespace Europium.Services.Apis.TheMovieDb;

public class Movie
{
	public int Id { get; set; }
	
	public string Title { get; set; } = null!;
	
	[JsonProperty("vote_average")]
	public float VoteAverage { get; set; }
	
	[JsonProperty("original_title")]
	public string OriginalTitle { get; set; } = null!;
	
	[JsonProperty("poster_path")]
	public string PosterPath { get; set; } = null!;
	
	[JsonProperty("backdrop_path")]
	public string BackdropPath { get; set; } = null!;
}