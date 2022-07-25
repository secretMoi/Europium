using Europium.Models;
using Europium.Services.Ssh;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using File = Europium.Models.File;

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
	
	[HttpGet("filesystems")]
	public async Task<IActionResult> GetFileSystems()
	{
		var listVolumesService = new ListVolumesService(AppConfig.SshHost, AppConfig.SshUser, AppConfig.SshPassword, AppConfig.SshPort);

		return Ok(await listVolumesService.GetFileSystemsAsync());
	}
	
	[HttpGet("files")]
	public async Task<IActionResult> GetFilesFromPath([FromBody]ListFilesArguments listFilesArguments)
	{
		var listFilesService = new ListFilesService(AppConfig.SshHost, AppConfig.SshUser, AppConfig.SshPassword, AppConfig.SshPort);

		return Ok(await listFilesService.GetFiles(listFilesArguments));
	}
}