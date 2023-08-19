using System.Data;
using System.Text;
using Europium.Mappers;
using Europium.Mappers.Plex;
using Europium.Repositories;
using Europium.Repositories.Auth;
using Europium.Repositories.FlareSolver;
using Europium.Repositories.FlareSolver.Models;
using Europium.Repositories.Ssh;
using Europium.Repositories.TheMovieDb;
using Europium.Services.Apis;
using Europium.Services.Apis.FlareSolver;
using Europium.Services.Apis.QBitTorrent;
using Europium.Services.Apis.TheMovieDb;
using Europium.Services.Apis.YggTorrent;
using Europium.Services.LocalDrives;
using Europium.Services.Ssh;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Europium.Helpers.Extensions;

public static class ServiceCollectionExtension
{
	public static void SetupCors(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(
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
	
	public static IServiceCollection AddPlex(this IServiceCollection services)
	{
		services.AddScoped<PlexMapper>();
		services.AddScoped<PlexDeviceMapper>();
		services.AddScoped<PlexUserMapper>();
		services.AddScoped<PlexSessionMapper>();
		services.AddScoped<PlexHistoryMapper>();
		services.AddScoped<PlexRepository>();
		services.AddScoped<PlexService>();
		
		return services;
	}
	
	public static IServiceCollection AddYgg(this IServiceCollection services)
	{
		services.AddScoped<YggTorrentRepository>();
		services.AddScoped<YggTorrentRatioFetcher>();
		services.AddScoped<YggMapper>();
		services.AddScoped<YggTorrentSearcher>();
		services.AddScoped<YggTorrentService>();
		
		return services;
	}
	
	public static IServiceCollection AddFlareSolver(this IServiceCollection services)
	{
		services.AddScoped<FlareSolverCommandFactory>();
		services.AddScoped<FlareSolverRepository>();
		services.AddScoped<FlareSolverService>();
		
		return services;
	}
	
	public static IServiceCollection AddDatabaseRepositories(this IServiceCollection services)
	{
		services.AddScoped<ConfigurationSettingRepository>();
		services.AddScoped<ApisToMonitorRepository>();
		services.AddScoped<ApiUrlRepository>();
		services.AddScoped<RefreshTokenRepository>();
		
		return services;
	}
	
	public static IServiceCollection AddQBitTorrent(this IServiceCollection services)
	{
		services.AddScoped<QBitTorrentRepository>();
		services.AddScoped<QBitTorrentService>();
		
		return services;
	}
	
	public static IServiceCollection AddTheMovieDatabase(this IServiceCollection services)
	{
		services.AddScoped<TheMovieDbRepository>();
		services.AddScoped<TheMovieDbService>();
		
		return services;
	}
	
	public static IServiceCollection AddRadarr(this IServiceCollection services)
	{
		services.AddScoped<RadarrRepository>();
		services.AddScoped<RadarrService>();
		
		return services;
	}
	
	public static IServiceCollection AddSonarr(this IServiceCollection services)
	{
		services.AddScoped<SonarrRepository>();
		services.AddScoped<SonarrService>();
		
		return services;
	}
	
	public static IServiceCollection AddTautulli(this IServiceCollection services)
	{
		services.AddScoped<TautulliService>();
		
		return services;
	}
	
	public static IServiceCollection AddJackett(this IServiceCollection services)
	{
		services.AddScoped<JackettService>();
		
		return services;
	}
	
	public static IServiceCollection AddNas(this IServiceCollection services)
	{
		services.AddScoped<SshNasRepository>();
		services.AddScoped<SshListFiles>();
		
		return services;
	}
	
	public static IServiceCollection AddDrives(this IServiceCollection services)
	{
		services.AddScoped<ListVolumesService>();
		services.AddScoped<LocalDrivesService>();
		
		return services;
	}
	
	public static IServiceCollection AddMedias(this IServiceCollection services)
	{
		services.AddScoped<SerieMapper>();
		services.AddScoped<SerieRepository>();
		services.AddScoped<SerieService>();
		services.AddScoped<MovieService>();
		
		return services;
	}
	
	public static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["AuthConfig:Issuer"],
					ValidAudience = configuration["AuthConfig:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Key"] ?? throw new NoNullAllowedException()))
				};
			}
		);
		
		services.AddAuthorization();
		
		return services;
	}
}