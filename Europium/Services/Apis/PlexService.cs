﻿using System.Diagnostics;
using Europium.Dtos.Plex;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class PlexService
{
	private readonly PlexRepository _plexRepository;

	public PlexService(PlexRepository plexRepository)
	{
		_plexRepository = plexRepository;
	}
	
	public async Task<bool?> IsUpAsync(string url)
	{
		return await _plexRepository.IsUpAsync(url);
	}

	public async Task<List<PlexDuplicateDto>> GetDuplicates(PlexLibraryType libraryType, int libraryId)
	{
		return await _plexRepository.GetDuplicates(libraryType, libraryId);
	}

	public async Task<List<PlexLibraryDto>> GetLibraries()
	{
		return await _plexRepository.GetLibraries();
	}

	public async Task<bool> DeleteMedia(int mediaId, int fileId)
	{
		return await _plexRepository.DeleteMedia(mediaId, fileId);
	}

	public async Task<Stream> GetThumbnail(int parentId, int thumbnailId)
	{
		return await _plexRepository.GetThumbnail(parentId, thumbnailId);
	}

	public bool Restart()
	{
		var processName = "Notepad";

		var processes = Process.GetProcessesByName(processName);
		if (processes.Length == 0)
			return false;

		var process = processes.First();
		var processPath = process.MainModule!.FileName;
		process.CloseMainWindow();
		if (!process.WaitForExit(2000))
			process.Kill();

		Process.Start(processPath!);

		var restartedProcesses = Process.GetProcessesByName(processName);
		return restartedProcesses.Length > 0;
	}
	
	// var process = new Process();
	// var startInfo = new ProcessStartInfo
	// {
	// 	WindowStyle = ProcessWindowStyle.Hidden,
	// 	FileName = "cmd.exe",
	// 	Arguments = "/C (TASKKILL /f /im \"notepad.exe\") & (start notepad.exe)",
	// 	RedirectStandardError = true,
	// 	RedirectStandardOutput = true
	// };
	// process.StartInfo = startInfo;
	// process.Start();
	// using var standardError = process.StandardError;
	// var t = await standardError.ReadLineAsync();
	// 	return (t).Length == 0;
}