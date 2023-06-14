using Europium.Dtos;
using Europium.Mappers;

namespace Europium.Services.LocalDrives;

public class LocalDrivesService
{
	public List<FileSystem> GetLocalDrives()
	{
		return DriveInfo.GetDrives().Select(driveInfo => new FileSystem
		{
			Size = driveInfo.TotalSize, 
			PercentageUsed = (int)((float)(driveInfo.TotalSize - driveInfo.TotalFreeSpace) / driveInfo.TotalSize * 100) + "%",
			Available = driveInfo.TotalFreeSpace,
			Used = driveInfo.TotalSize - driveInfo.TotalFreeSpace,
			Volume = driveInfo.Name + driveInfo.VolumeLabel,
			IsLocal = true
		}).ToList();
	}
}