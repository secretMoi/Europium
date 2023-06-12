using Europium.Dtos;
using Europium.Mappers;

namespace Europium.Services.LocalDrives;

public class LocalDrivesService
{
	private readonly SizeMapper _sizeMapper;

	public LocalDrivesService(SizeMapper sizeMapper)
	{
		_sizeMapper = sizeMapper;
	}
	
	public List<FileSystem> GetLocalDrives()
	{
		return DriveInfo.GetDrives().Select(driveInfo => new FileSystem
		{
			Size = _sizeMapper.ByteToValue(driveInfo.TotalSize, SizeMapper.SizeUnits.TB) + "T",
			PercentageUsed = (int)((float)(driveInfo.TotalSize - driveInfo.TotalFreeSpace) / driveInfo.TotalSize * 100) + "%",
			Available = _sizeMapper.ByteToValue(driveInfo.TotalFreeSpace, SizeMapper.SizeUnits.TB) + "T",
			Used = _sizeMapper.ByteToValue(driveInfo.TotalSize - driveInfo.TotalFreeSpace, SizeMapper.SizeUnits.TB) + "T",
			Volume = driveInfo.Name + driveInfo.VolumeLabel,
			IsLocal = true
		}).ToList();
	}
}