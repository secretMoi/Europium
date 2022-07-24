using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
	private readonly AppConfig AppConfig;

	public StorageController(IOptions<AppConfig> optionsSnapshot)
	{
		AppConfig = optionsSnapshot.Value;
	}
	
	[HttpGet]
	public async Task<List<FileSystem>> Get2()
	{
		var Ssh = new SSH(AppConfig.SshHost, AppConfig.SshUser, AppConfig.SshPassword, AppConfig.SshPort);
		await Ssh.ConnectAsync();
		var result = await Ssh.RunCommandAsync("df -h");
		return GereEspace(result);
	}

	private List<FileSystem> GereEspace(string result)
	{
		
		var parseCommandDf = new ParseCommandDf();
		
		var fileSystems = parseCommandDf.Parse(result);

		return fileSystems;
	}
}