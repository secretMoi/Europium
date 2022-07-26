using Europium.Models;

namespace Europium.Services.Ssh;

internal class ListVolumesService : SSHService
{
	private enum Colonnes
	{
		Total = 1, Utilise = 2, Libre = 3, Utilisation = 4, Nom = 5
	}

	public ListVolumesService(string host, string user, string password, int port = 22) : base(host, user, password, port)
	{
	}

	public async Task<List<FileSystem>> GetFileSystemsAsync()
	{
		await ConnectAsync();
		var result = await RunCommandAsync("df -h");

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
			else if (ValidColumn(idColonne, Colonnes.Utilise))
			{
				fileSystem.Used = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Utilisation))
			{
				fileSystem.PercentageUsed = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Nom))
			{
				fileSystem.Volume = item;
			}
			else if (ValidColumn(idColonne, Colonnes.Libre))
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
