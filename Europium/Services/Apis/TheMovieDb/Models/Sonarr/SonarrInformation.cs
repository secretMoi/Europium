namespace Europium.Services.Apis.TheMovieDb.Models.Sonarr;

public class Image
{
    public string CoverType { get; set; }
    public string Url { get; set; }
}

public class Ratings
{
    public int Votes { get; set; }
    public double Value { get; set; }
}

public class SonarrInformation
{
    public string FileLink { get; set; }
    public string Title { get; set; }
    public string SortTitle { get; set; }
    public int SeasonCount { get; set; }
    public string Status { get; set; }
    public string Overview { get; set; }
    public string Network { get; set; }
    public string AirTime { get; set; }
    public List<Image> Images { get; set; }
    public string RemotePoster { get; set; }
    public List<Season> Seasons { get; set; }
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
    public string SeriesType { get; set; }
    public string CleanTitle { get; set; }
    public string ImdbId { get; set; }
    public string TitleSlug { get; set; }
    public string Certification { get; set; }
    public List<string> Genres { get; set; }
    public List<object> Tags { get; set; }
    public DateTime Added { get; set; }
    public Ratings Ratings { get; set; }
    public int QualityProfileId { get; set; }
}

public class Season
{
    public int SeasonNumber { get; set; }
    public bool Monitored { get; set; }
}