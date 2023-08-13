using Europium;
using Europium.Helpers.Extensions;
using Europium.Mappers;
using Europium.Models;
using Europium.Repositories;
using Europium.Services.Apis;
using Europium.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appconfig.json", false, true);

builder.Services.AddDbContext<EuropiumContext>(opt => opt.UseSqlServer());

const string policyName = "_myAllowSpecificOrigins";
builder.Services.SetupCors(builder.Configuration, policyName);

builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(s =>
	{
		s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	}
);

builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddAutoMapper(typeof(Program))
	.Configure<AppConfig>(builder.Configuration)
	.AddSingleton<ConfigProgram>()
	.AddScoped<AuthService>()
	.AddScoped<SizeMapper>()
	.AddDatabseRepositories()
	.AddFlareSolver()
	.AddPlex()
	.AddTheMovieDatabase()
	.AddMedias()
	.AddNas()
	.AddDrives()
	.AddScoped<CommonApiRepository>()
	.AddScoped<MonitorService>()
	.AddJackett()
    .AddQBitTorrent()
    .AddRadarr()
    .AddSonarr()
    .AddTautulli()
    .AddYgg();

var app = builder.Build();

app.SetupPipeline(builder, policyName);