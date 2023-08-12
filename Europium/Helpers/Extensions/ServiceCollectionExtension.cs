using Europium.Mappers;
using Europium.Mappers.Plex;
using Europium.Repositories;
using Europium.Repositories.Auth;
using Europium.Repositories.FlareSolver;
using Europium.Repositories.FlareSolver.Models;
using Europium.Repositories.TheMovieDb;
using Europium.Services.Apis;
using Europium.Services.Apis.FlareSolver;
using Europium.Services.Apis.QBitTorrent;
using Europium.Services.Apis.TheMovieDb;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Helpers.Extensions;

public static class ServiceCollectionExtension
{
	public static void SetupCors(this IServiceCollection services, ConfigurationManager configuration)
	{
		const string myAllowSpecificOrigins = "_myAllowSpecificOrigins"; // todo
		services.AddCors(options =>
		{
			options.AddPolicy(name: myAllowSpecificOrigins,
				policy  =>
				{
					policy.WithOrigins(
						configuration.GetSection("AllowedUrls").Get<string[]>()
					);
					policy.WithMethods("GET", "POST", "DELETE", "PUT");
					policy.WithHeaders("authorization", "accept", "content-type", "origin");
				});
		});
	}
	
	public static void AddPlex(this IServiceCollection services)
	{
		services.AddScoped<PlexMapper>();
		services.AddScoped<PlexDeviceMapper>();
		services.AddScoped<PlexUserMapper>();
		services.AddScoped<PlexSessionMapper>();
		services.AddScoped<PlexHistoryMapper>();
		services.AddScoped<PlexRepository>();
		services.AddScoped<PlexService>();
	}
	
	public static void AddYgg(this IServiceCollection services)
	{
		services.AddScoped<YggTorrentRepository>();
		services.AddScoped<YggTorrentRatioFetcher>();
		services.AddScoped<YggMapper>();
		services.AddScoped<YggTorrentSearcher>();
		services.AddScoped<YggTorrentService>();
	}
	
	public static void AddFlareSolver(this IServiceCollection services)
	{
		services.AddScoped<FlareSolverCommandFactory>();
		services.AddScoped<FlareSolverRepository>();
		services.AddScoped<FlareSolverService>();
	}
	
	public static void AddDatabseRepositories(this IServiceCollection services)
	{
		services.AddScoped<ConfigurationSettingRepository>();
		services.AddScoped<ApisToMonitorRepository>();
		services.AddScoped<ApiUrlRepository>();
	}
	
	public static void AddQBitTorrent(this IServiceCollection services)
	{
		services.AddScoped<QBitTorrentRepository>();
		services.AddScoped<QBitTorrentService>();
	}
	
	public static void AddTheMovieDatabase(this IServiceCollection services)
	{
		services.AddScoped<TheMovieDbRepository>();
		services.AddScoped<TheMovieDbService>();
	}
}