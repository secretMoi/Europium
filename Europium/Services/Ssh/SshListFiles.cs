using Europium.Dtos;
using Europium.Repositories.Ssh;
using File = Europium.Dtos.File;

namespace Europium.Services.Ssh;

public class SshListFiles
{
	private readonly SshNasRepository _sshNasRepository;

	public SshListFiles(SshNasRepository sshNasRepository)
	{
		_sshNasRepository = sshNasRepository;
	}

	public async Task<List<File>?> GetFiles(ListFilesArguments listFilesArguments)
	{
		await _sshNasRepository.ConnectAsync();

		var commandResponse = await _sshNasRepository.RunCommandAsync(
			_sshNasRepository.GetSshCommandToExecute(listFilesArguments.Path ?? "", listFilesArguments.FileType,
				listFilesArguments.Limit));
		
		return commandResponse is null ? null : ParseCommandResponse(commandResponse);
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
}