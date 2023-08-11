using System.Data;
using System.Text;
using Europium;
using Europium.Mappers;
using Europium.Mappers.Plex;
using Europium.Models;
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
using Europium.Services.Auth;
using Europium.Services.LocalDrives;
using Europium.Services.Ssh;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(s =>
	{
		s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	}
);

builder.Configuration.AddJsonFile("appconfig.json", false, true);

builder.Services.AddDbContext<EuropiumContext>(opt => opt.UseSqlServer());

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: myAllowSpecificOrigins,
		policy  =>
		{
			policy.WithOrigins(
				builder.Configuration.GetSection("AllowedUrls").Get<string[]>()
				
				);
			policy.WithMethods("GET", "POST", "DELETE", "PUT");
			policy.WithHeaders("authorization", "accept", "content-type", "origin");
		});
});

builder.Services.AddScoped<ConfigurationSettingRepository>();
builder.Services.AddAuthentication(x =>
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
			ValidIssuer = builder.Configuration["AuthConfig:Issuer"],
			ValidAudience = builder.Configuration["AuthConfig:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthConfig:Key"] ?? throw new NoNullAllowedException()))
		};
	}
);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<AppConfig>(builder.Configuration);

builder.Services.AddSingleton<ConfigProgram>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApisToMonitorRepository>();
builder.Services.AddScoped<ApiUrlRepository>();

builder.Services.AddScoped<SizeMapper>();

builder.Services.AddScoped<FlareSolverCommandFactory>();
builder.Services.AddScoped<FlareSolverRepository>();
builder.Services.AddScoped<FlareSolverService>();

builder.Services.AddScoped<PlexMapper>();
builder.Services.AddScoped<PlexDeviceMapper>();
builder.Services.AddScoped<PlexUserMapper>();
builder.Services.AddScoped<PlexSessionMapper>();
builder.Services.AddScoped<PlexHistoryMapper>();
builder.Services.AddScoped<PlexRepository>();
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

builder.Services.AddScoped<CommonApiRepository>();
builder.Services.AddScoped<MonitorService>();
builder.Services.AddScoped<JackettService>();
builder.Services.AddScoped<QBitTorrentRepository>();
builder.Services.AddScoped<QBitTorrentService>();

builder.Services.AddScoped<RadarrRepository>();
builder.Services.AddScoped<RadarrService>();

builder.Services.AddScoped<SonarrRepository>();
builder.Services.AddScoped<SonarrService>();

builder.Services.AddScoped<TautulliService>();

builder.Services.AddScoped<YggTorrentRepository>();
builder.Services.AddScoped<YggTorrentRatioFetcher>();
builder.Services.AddScoped<YggMapper>();
builder.Services.AddScoped<YggTorrentSearcher>();
builder.Services.AddScoped<YggTorrentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

var configProgram = app.Services.GetService<ConfigProgram>()!;
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.ContentRootPath, configProgram.RessourcesPath
	)),
	RequestPath = "/Ressources"
});

app.UseCors(myAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();