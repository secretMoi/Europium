using Europium.Models;
using Europium.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Europium.Repositories;

public class EuropiumContext : DbContext
{
	private readonly AppConfig _appConfig;

	public EuropiumContext(IOptions<AppConfig> optionsSnapshot)
	{
		_appConfig = optionsSnapshot.Value;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// connect to sql server with connection string from app settings
		optionsBuilder.UseSqlServer(_appConfig.EuropiumDatabase);
	}

	public DbSet<ApiToMonitor> ApisToMonitor { get; set; } = null!;
	public DbSet<ApiUrl> ApiUrls { get; set; } = null!;
	public DbSet<ConfigurationSetting> ConfigurationSettings { get; set; } = null!;
}