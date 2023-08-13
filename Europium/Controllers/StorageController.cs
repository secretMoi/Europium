using Europium.Dtos;
using Europium.Services.LocalDrives;
using Europium.Services.Ssh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using File = Europium.Dtos.File;

namespace Europium.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class StorageController : ControllerBase
{
	private readonly ListVolumesService _listVolumesService;
	private readonly SshListFiles _sshListFiles;
	private readonly LocalDrivesService _localDrivesService;

	public StorageController(ListVolumesService listVolumesService, SshListFiles sshListFiles, LocalDrivesService localDrivesService)
	{
		_listVolumesService = listVolumesService;
		_sshListFiles = sshListFiles;
		_localDrivesService = localDrivesService;
	}
	
	[HttpGet("filesystems")]
	public async Task<IActionResult> GetFileSystems()
	{
		List<FileSystem> volumes = new List<FileSystem>();
		volumes.AddRange(await _listVolumesService.GetFileSystemsAsync() ?? new List<FileSystem>());
		volumes.AddRange(_localDrivesService.GetLocalDrives());

		return Ok(volumes);
	}
	
	[HttpPost("files")]
	public async Task<IActionResult> GetFilesFromPath([FromBody]ListFilesArguments listFilesArguments)
	{
		IEnumerable<File> files = new List<File>();
		
		if(!listFilesArguments.IsLocal)
			files = await _sshListFiles.GetFiles(listFilesArguments) ?? new List<File>();
		if(listFilesArguments.IsLocal)
			files = await _localDrivesService.GetFiles(listFilesArguments);

		return Ok(files);
	}
}