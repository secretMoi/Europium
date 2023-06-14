using System.Diagnostics;
using Europium.Dtos;
using File = Europium.Dtos.File;

namespace Europium.Services.LocalDrives;

public class LocalDrivesService
{
    public IEnumerable<FileSystem> GetLocalDrives()
    {
        return DriveInfo.GetDrives().Select(driveInfo => new FileSystem
        {
            Size = driveInfo.TotalSize,
            PercentageUsed =
                (int)((float)(driveInfo.TotalSize - driveInfo.TotalFreeSpace) / driveInfo.TotalSize * 100) + "%",
            Available = driveInfo.TotalFreeSpace,
            Used = driveInfo.TotalSize - driveInfo.TotalFreeSpace,
            Volume = driveInfo.Name + driveInfo.VolumeLabel,
            IsLocal = true
        });
    }

    public async Task<IEnumerable<File>> GetFiles(ListFilesArguments listFilesArguments)
    {
        var commandResult = await ExecuteGetFilesCommand(listFilesArguments);
        var fileLines = FormatGetFilesCommandReturn(commandResult);
        return fileLines.Select(CreateFile);
    }

    private async Task<string> ExecuteGetFilesCommand(ListFilesArguments listFilesArguments)
    {
        using var app = new Process();
        app.StartInfo.FileName = "powershell.exe";
        app.StartInfo.Arguments = GetGetFilesCommand(listFilesArguments);
        app.EnableRaisingEvents = true;
        app.StartInfo.RedirectStandardOutput = true;
        app.StartInfo.RedirectStandardError = true;
        app.StartInfo.UseShellExecute = false; // Must not set true to execute PowerShell command
        app.Start();
        using var standardOutput = app.StandardOutput;
        return await standardOutput.ReadToEndAsync();
    }

    private IEnumerable<string> FormatGetFilesCommandReturn(string commandResult)
    {
        return commandResult
            .Split("\r\n")
            .Skip(3)
            .Where(file => file != string.Empty)
            .Select(file => file.Trim());
    }

    private File CreateFile(string fileLine)
    {
        var fileInformation = fileLine.Split(new[] { ' ' }, 2);
        return new File(fileInformation[1], long.Parse(fileInformation[0]));
    }

    private string GetGetFilesCommand(ListFilesArguments listFilesArguments)
    {
        string command;
        if (listFilesArguments.FileType == FileType.File)
            command =
                $"Get-ChildItem -Path {listFilesArguments.Path} -File -Recurse | Sort-Object -Property Length -Descending | Select-Object -Property Length, Name -first {listFilesArguments.Limit}";
        else
            command =
                $"$fso = new-object -com Scripting.FileSystemObject; Get-ChildItem -Path {listFilesArguments.Path} -Directory -Recurse | Select-Object @{{l='Size'; e={{$fso.GetFolder($_.FullName).Size}}}},FullName | Sort-Object Size -Descending | Select-Object -first {listFilesArguments.Limit}";

        return command;
    }
}