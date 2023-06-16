using Europium.Services.Apis.TheMovieDb.Models.Tmdb;

namespace Europium.Mappers;

public class SerieMapper
{
    public Media MapSerie(Media media, string imageBasePath, List<Season>? seasons)
    {
        media.Link = $"https://www.themoviedb.org/tv/{media.Id}?language=fr";
        media.Seasons = seasons;
        media.BackdropPath = imageBasePath + media.BackdropPath;
        media.PosterPath = imageBasePath + media.PosterPath;
        media.Title = media.Name;

        return media;
    }
}