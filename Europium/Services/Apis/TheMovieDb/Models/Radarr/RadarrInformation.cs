namespace Europium.Services.Apis.TheMovieDb.Models.Radarr;

public class AlternateTitle
{
	public string SourceType { get; set; } = null!;
	public int MovieMetadataId { get; set; }
	public string Title { get; set; } = null!;
	public int SourceId { get; set; }
	public int Votes { get; set; }
	public int VoteCount { get; set; }
	public Language Language { get; set; } = null!;
	public int Id { get; set; }
}

public class Image
{
	public string CoverType { get; set; } = null!;
	public string Url { get; set; } = null!;
	public string RemoteUrl { get; set; } = null!;
}

public class Imdb
{
	public int Votes { get; set; }
	public double Value { get; set; }
	public string Type { get; set; } = null!;
}

public class Language
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
}

public class MediaInfo
{
	public int AudioBitrate { get; set; }
	public double AudioChannels { get; set; }
	public string AudioCodec { get; set; } = null!;
	public string AudioLanguages { get; set; } = null!;
	public int AudioStreamCount { get; set; }
	public int VideoBitDepth { get; set; }
	public int VideoBitrate { get; set; }
	public string VideoCodec { get; set; } = null!;
	public string VideoDynamicRangeType { get; set; } = null!;
	public double VideoFps { get; set; }
	public string Resolution { get; set; } = null!;
	public string RunTime { get; set; } = null!;
	public string ScanType { get; set; } = null!;
	public string Subtitles { get; set; } = null!;
}

public class Metacritic
{
	public int Votes { get; set; }
	public int Value { get; set; }
	public string Type { get; set; } = null!;
}

public class MovieFile
{
	public int MovieId { get; set; }
	public string RelativePath { get; set; } = null!;
	public string Path { get; set; } = null!;
	public long Size { get; set; }
	public DateTime DateAdded { get; set; }
	public int IndexerFlags { get; set; }
	public QualityContainer Quality { get; set; } = null!;
	public MediaInfo MediaInfo { get; set; } = null!;
	public string OriginalFilePath { get; set; } = null!;
	public bool QualityCutoffNotMet { get; set; }
	public List<Language> Languages { get; set; } = null!;
	public string ReleaseGroup { get; set; } = null!;
	public string Edition { get; set; } = null!;
	public int Id { get; set; }
}

public class OriginalLanguage
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
}

public class QualityDetail
{
	public Revision Revision { get; set; } = null!;
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Source { get; set; } = null!;
	public int Resolution { get; set; }
	public string Modifier { get; set; } = null!;
}

public class QualityContainer
{
	public QualityDetail Quality { get; set; } = null!;
	public Revision Revision { get; set; } = null!;
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Source { get; set; } = null!;
	public int Resolution { get; set; }
	public string Modifier { get; set; } = null!;
}

public class Ratings
{
	public Imdb Imdb { get; set; } = null!;
	public Tmdb Tmdb { get; set; } = null!;
	public Metacritic Metacritic { get; set; } = null!;
	public RottenTomatoes RottenTomatoes { get; set; } = null!;
}

public class Revision
{
	public int Version { get; set; }
	public int Real { get; set; }
	public bool IsRepack { get; set; }
}

public class RadarrInformation
{
	public string FileLink { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string OriginalTitle { get; set; } = null!;
	public OriginalLanguage OriginalLanguage { get; set; } = null!;
	public List<AlternateTitle> AlternateTitles { get; set; } = null!;
	public int SecondaryYearSourceId { get; set; }
	public string SortTitle { get; set; } = null!;
	public long SizeOnDisk { get; set; }
	public string Status { get; set; } = null!;
	public string Overview { get; set; } = null!;
	public DateTime InCinemas { get; set; }
	public DateTime PhysicalRelease { get; set; }
	public DateTime DigitalRelease { get; set; }
	public List<Image> Images { get; set; } = null!;
	public string Website { get; set; } = null!;
	public int Year { get; set; }
	public bool HasFile { get; set; }
	public string YouTubeTrailerId { get; set; } = null!;
	public string Studio { get; set; } = null!;
	public string Path { get; set; } = null!;
	public int QualityProfileId { get; set; }
	public bool Monitored { get; set; }
	public string MinimumAvailability { get; set; } = null!;
	public bool IsAvailable { get; set; }
	public string FolderName { get; set; } = null!;
	public int Runtime { get; set; }
	public string CleanTitle { get; set; } = null!;
	public string ImdbId { get; set; } = null!;
	public int TmdbId { get; set; }
	public string TitleSlug { get; set; } = null!;
	public string Certification { get; set; } = null!;
	public List<string> Genres { get; set; } = null!;
	public List<object> Tags { get; set; } = null!;
	public DateTime Added { get; set; }
	public Ratings Ratings { get; set; } = null!;
	public MovieFile MovieFile { get; set; } = null!;
	public double Popularity { get; set; }
	public int Id { get; set; }
}

public class RottenTomatoes
{
	public int Votes { get; set; }
	public int Value { get; set; }
	public string Type { get; set; } = null!;
}

public class Tmdb
{
	public int Votes { get; set; }
	public double Value { get; set; }
	public string Type { get; set; } = null!;
}