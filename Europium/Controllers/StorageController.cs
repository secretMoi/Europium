using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
	[HttpGet(Name = "cou")]
	public async Task<List<FileSystem>> Get2()
	{
		var Ssh = new SSH();
		await Ssh.ConnectAsync();
		var result = await Ssh.RunCommandAsync("df -h");
		return GereEspace(result);
	}

	private List<FileSystem> GereEspace(String result)
	{
		
		var parseCommandDf = new ParseCommandDf();
		
		var fileSystems = parseCommandDf.Parse(result);
		
		Console.WriteLine("coucou");

		return fileSystems;
	}
}