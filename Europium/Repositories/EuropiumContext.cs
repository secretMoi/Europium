using Europium.Models;
using Europium.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Europium.Repositories;

public class EuropiumContext : DbContext
{
	private readonly AppConfig AppConfig;

	public EuropiumContext(IOptions<AppConfig> optionsSnapshot)
	{
		AppConfig = optionsSnapshot.Value;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// connect to sql server with connection string from app settings
		optionsBuilder.UseSqlServer(AppConfig.EuropiumDatabase);
	}

	public DbSet<ApiToMonitor?> ApisToMonitor { get; set; }
	public DbSet<ApiUrl> ApiUrls { get; set; }
}