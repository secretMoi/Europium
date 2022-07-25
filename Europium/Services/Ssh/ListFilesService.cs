using Europium.Models;
using File = Europium.Models.File;

namespace Europium.Services.Ssh;

public class ListFilesService : SSHService
{
	private ListFilesArguments _listFilesArguments;
	
	public ListFilesService(string host, string user, string password, int port = 22) : base(host, user, password, port)
	{
	}

	public async Task<List<File>> GetFiles(ListFilesArguments listFilesArguments)
	{
		_listFilesArguments = listFilesArguments;
		
		await ConnectAsync();
		var commandResponse = await RunCommandAsync(GetSshCommandToExecute());

		return ParseCommandResponse(commandResponse);
	}

	private List<File> ParseCommandResponse(string commandResponse)
	{
		var lines = commandResponse.Split('\n').SkipLast(1); // casse la chaine en lignes
		
		return lines.Select(GetFileFromCommandResponse).ToList();
	}

	private File GetFileFromCommandResponse(string commandResponse)
	{
		int columnIndex = 0; // renvoie le n° de la colonne actuel

		var file = new File();

		// split en colonne par espace
		foreach (string item in commandResponse.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
		{
			switch (columnIndex)
			{
				case 0:
					file.Size = Convert.ToInt32(item);
					break;
				case 1:
					file.Path = item;
					columnIndex = 0;
					break;
			}

			columnIndex++;
		}

		return file;
	}

	private string GetSshCommandToExecute()
	{
		var commandToExecute = "find " + _listFilesArguments.Path;

		if (_listFilesArguments.FileType == FileType.File)
			commandToExecute += " -type f";
		if (_listFilesArguments.FileType == FileType.Folder)
			commandToExecute += " -type d";

		commandToExecute += " -exec du -S {} + | sort -rh | head -n " + _listFilesArguments.Limit;

		return commandToExecute;
	}
}