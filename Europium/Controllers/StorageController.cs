using Europium.Dtos;
using Europium.Services.Ssh;
using Microsoft.AspNetCore.Mvc;
using File = Europium.Dtos.File;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
	private readonly ListVolumesService _listVolumesService;
	private readonly ListFilesService _listFilesService;

	public StorageController(ListVolumesService listVolumesService, ListFilesService listFilesService)
	{
		_listVolumesService = listVolumesService;
		_listFilesService = listFilesService;
	}
	
	[HttpGet("filesystems")]
	public async Task<IActionResult> GetFileSystems()
	{
		var volumes = await _listVolumesService.GetFileSystemsAsync();

		if (volumes is null || volumes.Count == 0)
			return NotFound();

		return Ok(volumes);
	}
	
	[HttpPost("files")]
	public async Task<IActionResult> GetFilesFromPath([FromBody]ListFilesArguments listFilesArguments)
	{
		var files = await _listFilesService.GetFiles(listFilesArguments);
		
		if (files is null || files.Count == 0)
			return NotFound();

		return Ok(files);
	}
}