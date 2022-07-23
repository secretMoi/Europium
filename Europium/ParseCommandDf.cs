namespace Europium;

class ParseCommandDf
{
	private readonly List<int> _colonnesSelectionnees; // liste des colonnes demandées
	private readonly Dictionary<Colonnes, int> _ordreColonnes; // associe une colonne à son numéro d'ordre pour le tableau

	public enum Colonnes
	{
		Total = 1, Utilise = 2, Libre = 3, Utilisation = 4, Nom = 5
	}

	public ParseCommandDf()
	{
		_colonnesSelectionnees= new List<int>();
		_ordreColonnes = new Dictionary<Colonnes, int>();
	}

       public ParseCommandDf(params Colonnes[] colonnes) : this()
       {
		AddColonnes(colonnes);
       }

	public void AddColonnes(params Colonnes[] colonnes)
	{
		foreach (Colonnes colonne in colonnes)
		{
			if (Enum.IsDefined(typeof(Colonnes), colonne))
			{
				_ordreColonnes.Add(colonne, _colonnesSelectionnees.Count);
				_colonnesSelectionnees.Add((int)colonne);
			}
		}
	}

	public List<FileSystem> Parse(string retourCommandeBrut)
	{
		string[] lines = retourCommandeBrut.Split('\n'); // casse la chaine en lignes

		var fileSystems = new List<FileSystem>();

		foreach (string ligne in lines)
		{

			// -1 pour la ligne de header qu'on retire
			fileSystems.Add(ParseBySpace(ligne));
		}

		return fileSystems;
	}

	private FileSystem ParseBySpace(string ligne)
	{
		int compteurColonne = 0; // renvoie le n° de la colonne actuel
		int idColonne; // permet d'identifier la colonne

		FileSystem fileSystem = new FileSystem();

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
				fileSystem.Volume = item;
			}
			
			compteurColonne++;
		}

		return fileSystem;
	}

	// si la colonne est sélectionnée et que son id correspond
	private bool ValidColumn(int id, Colonnes colonne)
	{
		return _colonnesSelectionnees.Contains(id) && id == (int) colonne;
	}

	// permet d'obtenir le n° d'ordre de la colonne
       public int GetColumn(Colonnes colonne)
       {
           return _ordreColonnes[colonne];
       }

       public static string CleanName(string name)
       {
           name = name.Substring(1);

           if (name.Contains("/usbshare"))
               name = name.Remove(name.Length - "/usbshare".Length, "/usbshare".Length);

           return name;
	}
}
