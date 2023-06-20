using System.Globalization;
using Europium.Helpers.Extensions;

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

	public long ValueToByte(string value)
	{
		if (value.Contains("G")) return (long)(double.Parse(value.GetOnlyNumeric(), CultureInfo.InvariantCulture) * Math.Pow(1024, 3));
		if (value.Contains("M")) return (long)(double.Parse(value.GetOnlyNumeric(), CultureInfo.InvariantCulture) * Math.Pow(1024, 2));
		if (value.Contains("K")) return (long)(double.Parse(value.GetOnlyNumeric(), CultureInfo.InvariantCulture) * 1024);

		return long.Parse(value.GetOnlyNumeric());
	}
}