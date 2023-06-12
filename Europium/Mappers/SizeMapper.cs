namespace Europium.Mappers;

public class SizeMapper
{
	public enum SizeUnits
	{
		Byte, KB, MB, GB, TB, PB, EB, ZB, YB
	}
	
	public string ByteToValue(long value, SizeUnits unit)
	{
		return (value / Math.Pow(1024, (long)unit)).ToString("0.0");
	}
}