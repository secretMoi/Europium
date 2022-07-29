using Europium.Models;
using Microsoft.Extensions.Options;

namespace Europium;

public class ConfigProgram
{
	private readonly AppConfig AppConfig;

	public ConfigProgram(IOptions<AppConfig> optionsSnapshot)
	{
		AppConfig = optionsSnapshot.Value;
	}

	public string RessourcesPath => AppConfig.ApiToMonitorImagePath;
}