namespace Europium;

internal class ParseCommandDf
{
	private enum Colonnes
	{
		Total = 1, Utilise = 2, Libre = 3, Utilisation = 4, Nom = 5
	}

	public List<FileSystem> Parse(string retourCommandeBrut)
	{
		var lines = retourCommandeBrut.Split('\n').SkipLast(1).Skip(1); // casse la chaine en lignes

		return lines.Select(ParseBySpace).ToList();
	}

	private FileSystem ParseBySpace(string ligne)
	{
		int compteurColonne = 0; // renvoie le n° de la colonne actuel
		int idColonne; // permet d'identifier la colonne

		var fileSystem = new FileSystem();

		// split en colonne par espace
		foreach (string item in ligne.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
		{
			idColonne = compteurColonne % 6;

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
				fileSystem.Volume = CleanName(item);
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

	public static string CleanName(string name)
    {
        name = name.Substring(1);

        if (name.Contains("/usbshare"))
            name = name.Remove(name.Length - "/usbshare".Length, "/usbshare".Length);

        return name;
	}
}
