using Europium.Dtos;
using Europium.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services.Ssh;

public class ListVolumesService : SshService
{
	private enum Colonnes
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

	private FileSystem ParseBySpace(string ligne)
	{
		int compteurColonne = 0; // renvoie le n° de la colonne actuel

		var fileSystem = new FileSystem();

		// split en colonne par espace
		foreach (string item in ligne.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
		{
			var idColonne = compteurColonne % 6; // permet d'identifier la colonne

			if (ValidColumn(idColonne, Colonnes.Total))
			{
				fileSystem.Size = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Used))
			{
				fileSystem.Used = item;
			}
			else if (ValidColumn(idColonne, Colonnes.PercentageUsed))
			{
				fileSystem.PercentageUsed = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Name))
			{
				fileSystem.Volume = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Free))
			{
				fileSystem.Available = item;
			}

			compteurColonne++;
		}

		return fileSystem;
	}

	// si la colonne est sélectionnée et que son id correspond
	private bool ValidColumn(int id, Colonnes colonne)
	{
		return id == (int) colonne;
	}
}
