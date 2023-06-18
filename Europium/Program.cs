using Europium;
using Europium.Mappers;
using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Ssh;
using Europium.Repositories.TheMovieDb;
using Europium.Services.Apis;
using Europium.Services.Apis.QBitTorrent;
using Europium.Services.Apis.TheMovieDb;
using Europium.Services.Apis.YggTorrent;
using Europium.Services.LocalDrives;
using Europium.Services.Ssh;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Plex.Api.Factories;
using Plex.Library.Factories;
using Plex.ServerApi;
using Plex.ServerApi.Api;
using Plex.ServerApi.Clients;
using Plex.ServerApi.Clients.Interfaces;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
	s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
	s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}
);

builder.Configuration.AddJsonFile("appconfig.json", false, true);

// initialise le service de connexion à la bdd
builder.Services.AddDbContext<EuropiumContext>(opt => opt.UseSqlServer());

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
		policy  =>
		{
			policy.WithOrigins(
				builder.Configuration.GetSection("AllowedUrls").Get<string[]>()
				
				);
			policy.WithHeaders("authorization", "accept", "content-type", "origin");
		});
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<AppConfig>(builder.Configuration);

builder.Services.AddSingleton<ConfigProgram>();
builder.Services.AddScoped<ApisToMonitorRepository>();
builder.Services.AddScoped<ApiUrlRepository>();

// Create Client Options
var apiOptions = new ClientOptions
{
	Product = "API_UnitTests",
	DeviceName = "API_UnitTests",
	ClientId = "MyClientId",
	Platform = "Web",
	Version = "v1"
};
builder.Services.AddSingleton(apiOptions);
builder.Services.AddScoped<SizeMapper>();

builder.Services.AddScoped<IPlexServerClient, PlexServerClient>();
builder.Services.AddScoped<IPlexAccountClient, PlexAccountClient>();
builder.Services.AddScoped<IPlexLibraryClient, PlexLibraryClient>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IPlexFactory, PlexFactory>();
builder.Services.AddScoped<IPlexRequestsHttpClient, PlexRequestsHttpClient>();
builder.Services.AddScoped<PlexService>();

builder.Services.AddScoped<TheMovieDbRepository>();
builder.Services.AddScoped<TheMovieDbService>();

builder.Services.AddScoped<MovieService>();

builder.Services.AddScoped<SerieMapper>();
builder.Services.AddScoped<SerieRepository>();
builder.Services.AddScoped<SerieService>();

builder.Services.AddScoped<SshNasRepository>();
builder.Services.AddScoped<SshListFiles>();
builder.Services.AddScoped<ListVolumesService>();
builder.Services.AddScoped<LocalDrivesService>();

builder.Services.AddScoped<CommonApiService>();
builder.Services.AddScoped<MonitorService>();
builder.Services.AddScoped<JackettService>();
builder.Services.AddScoped<QBitTorrentService>();

builder.Services.AddScoped<RadarrRepository>();
builder.Services.AddScoped<RadarrService>();

builder.Services.AddScoped<SonarrRepository>();
builder.Services.AddScoped<SonarrService>();

builder.Services.AddScoped<TautulliService>();

builder.Services.AddScoped<YggTorrentRepository>();
builder.Services.AddScoped<YggTorrentService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var configProgram = app.Services.GetService<ConfigProgram>()!;
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.ContentRootPath, configProgram.RessourcesPath
	)),
	RequestPath = "/Ressources"
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();