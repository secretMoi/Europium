namespace Europium.Services.Apis.TheMovieDb.Models;

public class AlternateTitle
{
	public string SourceType { get; set; }
	public int MovieMetadataId { get; set; }
	public string Title { get; set; }
	public int SourceId { get; set; }
	public int Votes { get; set; }
	public int VoteCount { get; set; }
	public Language Language { get; set; }
	public int Id { get; set; }
}

public class Image
{
	public string CoverType { get; set; }
	public string Url { get; set; }
	public string RemoteUrl { get; set; }
}

public class Imdb
{
	public int Votes { get; set; }
	public double Value { get; set; }
	public string Type { get; set; }
}

public class Language
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class MediaInfo
{
	public int AudioBitrate { get; set; }
	public double AudioChannels { get; set; }
	public string AudioCodec { get; set; }
	public string AudioLanguages { get; set; }
	public int AudioStreamCount { get; set; }
	public int VideoBitDepth { get; set; }
	public int VideoBitrate { get; set; }
	public string VideoCodec { get; set; }
	public string VideoDynamicRangeType { get; set; }
	public double VideoFps { get; set; }
	public string Resolution { get; set; }
	public string RunTime { get; set; }
	public string ScanType { get; set; }
	public string Subtitles { get; set; }
}

public class Metacritic
{
	public int Votes { get; set; }
	public int Value { get; set; }
	public string Type { get; set; }
}

public class MovieFile
{
	public int MovieId { get; set; }
	public string RelativePath { get; set; }
	public string Path { get; set; }
	public long Size { get; set; }
	public DateTime DateAdded { get; set; }
	public int IndexerFlags { get; set; }
	public QualityContainer Quality { get; set; }
	public MediaInfo MediaInfo { get; set; }
	public string OriginalFilePath { get; set; }
	public bool QualityCutoffNotMet { get; set; }
	public List<Language> Languages { get; set; }
	public string ReleaseGroup { get; set; }
	public string Edition { get; set; }
	public int Id { get; set; }
}

public class OriginalLanguage
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class QualityDetail
{
	public Revision Revision { get; set; }
	public int Id { get; set; }
	public string Name { get; set; }
	public string Source { get; set; }
	public int Resolution { get; set; }
	public string Modifier { get; set; }
}

public class QualityContainer
{
	public QualityDetail Quality { get; set; }
	public Revision Revision { get; set; }
	public int Id { get; set; }
	public string Name { get; set; }
	public string Source { get; set; }
	public int Resolution { get; set; }
	public string Modifier { get; set; }
}

public class Ratings
{
	public Imdb Imdb { get; set; }
	public Tmdb Tmdb { get; set; }
	public Metacritic Metacritic { get; set; }
	public RottenTomatoes RottenTomatoes { get; set; }
}

public class Revision
{
	public int Version { get; set; }
	public int Real { get; set; }
	public bool IsRepack { get; set; }
}

public class RadarrInformation
{
	public string FileLink { get; set; }
	public string Title { get; set; }
	public string OriginalTitle { get; set; }
	public OriginalLanguage OriginalLanguage { get; set; }
	public List<AlternateTitle> AlternateTitles { get; set; }
	public int SecondaryYearSourceId { get; set; }
	public string SortTitle { get; set; }
	public long SizeOnDisk { get; set; }
	public string Status { get; set; }
	public string Overview { get; set; }
	public DateTime InCinemas { get; set; }
	public DateTime PhysicalRelease { get; set; }
	public DateTime DigitalRelease { get; set; }
	public List<Image> Images { get; set; }
	public string Website { get; set; }
	public int Year { get; set; }
	public bool HasFile { get; set; }
	public string YouTubeTrailerId { get; set; }
	public string Studio { get; set; }
	public string Path { get; set; }
	public int QualityProfileId { get; set; }
	public bool Monitored { get; set; }
	public string MinimumAvailability { get; set; }
	public bool IsAvailable { get; set; }
	public string FolderName { get; set; }
	public int Runtime { get; set; }
	public string CleanTitle { get; set; }
	public string ImdbId { get; set; }
	public int TmdbId { get; set; }
	public string TitleSlug { get; set; }
	public string Certification { get; set; }
	public List<string> Genres { get; set; }
	public List<object> Tags { get; set; }
	public DateTime Added { get; set; }
	public Ratings Ratings { get; set; }
	public MovieFile MovieFile { get; set; }
	public double Popularity { get; set; }
	public int Id { get; set; }
}

public class RottenTomatoes
{
	public int Votes { get; set; }
	public int Value { get; set; }
	public string Type { get; set; }
}

public class Tmdb
{
	public int Votes { get; set; }
	public double Value { get; set; }
	public string Type { get; set; }
}