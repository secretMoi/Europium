namespace Europium.Services.Apis.TheMovieDb.Models.Sonarr;

public class Image
{
    public string CoverType { get; set; } = null!;
    public string Url { get; set; } = null!;
}

public class Ratings
{
    public int Votes { get; set; }
    public double Value { get; set; }
}

public class SonarrInformation
{
    public string FileLink { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string SortTitle { get; set; } = null!;
    public int SeasonCount { get; set; }
    public string Status { get; set; } = null!;
    public string Overview { get; set; } = null!;
    public string Network { get; set; } = null!;
    public string AirTime { get; set; } = null!;
    public List<Image> Images { get; set; } = null!;
    public string RemotePoster { get; set; } = null!;
    public List<Season> Seasons { get; set; } = null!;
    public int Year { get; set; }
    public int ProfileId { get; set; }
    public bool SeasonFolder { get; set; }
    public bool Monitored { get; set; }
    public bool UseSceneNumbering { get; set; }
    public int Runtime { get; set; }
    public int TvdbId { get; set; }
    public int TvRageId { get; set; }
    public int TvMazeId { get; set; }
    public DateTime FirstAired { get; set; }
    public string SeriesType { get; set; } = null!;
    public string CleanTitle { get; set; } = null!;
    public string ImdbId { get; set; } = null!;
    public string TitleSlug { get; set; } = null!;
    public string Certification { get; set; } = null!;
    public List<string> Genres { get; set; } = null!;
    public List<object> Tags { get; set; } = null!;
    public DateTime Added { get; set; }
    public Ratings Ratings { get; set; } = null!;
    public int QualityProfileId { get; set; }
}

public class Season
{
    public int SeasonNumber { get; set; }
    public bool Monitored { get; set; }
}