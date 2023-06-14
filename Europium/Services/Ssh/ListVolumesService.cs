using Europium.Dtos;
using Europium.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services.Ssh;

public class ListVolumesService : SshService
{
	private enum Column
	{
		Total = 1, Used = 2, Free = 3, PercentageUsed = 4, Name = 5
	}

	public ListVolumesService(IOptions<AppConfig> optionsSnapshot)
		: base(optionsSnapshot.Value.SshHost, optionsSnapshot.Value.SshUser, optionsSnapshot.Value.SshPassword, optionsSnapshot.Value.SshPort)
	{
	}

	public async Task<List<FileSystem>?> GetFileSystemsAsync()
	{
		await ConnectAsync();
		var result = await RunCommandAsync("df -h");
		if (result is null) return null;

		return ParseSshResponse(result);
	}

	private List<FileSystem> ParseSshResponse(string retourCommandeBrut)
	{
		var lines = retourCommandeBrut
			.Split('\n') // casse la chaine en lignes
			.Where(line => line.Contains("volume"));

		return lines.Select(ParseBySpace).ToList();
	}

	private FileSystem ParseBySpace(string line)
	{
		int columnIndex = 0; // renvoie le n° de la colonne actuel

		var fileSystem = new FileSystem();

		// split en colonne par espace
		foreach (string item in line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
		{
			var columnId = columnIndex % 6; // permet d'identifier la colonne

			if (ValidColumn(columnId, Column.Total))
			{
				fileSystem.Size = long.Parse(item);
			}
			else if (ValidColumn(columnId, Column.Used))
			{
				fileSystem.Used = long.Parse(item);
			}
			else if (ValidColumn(columnId, Column.PercentageUsed))
			{
				fileSystem.PercentageUsed = item;
			}
			else if (ValidColumn(columnId, Column.Name))
			{
				fileSystem.Volume = item;
			}
			else if (ValidColumn(columnId, Column.Free))
			{
				fileSystem.Available = long.Parse(item);
			}

			columnIndex++;
		}

		fileSystem.IsLocal = false;

		return fileSystem;
	}

	// si la colonne est sélectionnée et que son id correspond
	private bool ValidColumn(int id, Column column)
	{
		return id == (int) column;
	}
}
