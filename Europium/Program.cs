using Europium;
using Europium.Models;
using Europium.Repositories;
using Europium.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

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
builder.Services.AddScoped<MonitorService>();
builder.Services.AddScoped<RadarrService>();

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