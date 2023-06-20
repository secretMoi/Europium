using Europium.Helpers.Extensions;

namespace Europium.Mappers;

public class YggMapper
{
    public long MapTorrentAge(string age)
    {
        if (age.Contains("an")) return long.Parse(age.GetOnlyNumeric()) * 365 * 24 * 60 * 60;
        if (age.Contains("mois")) return long.Parse(age.GetOnlyNumeric()) * 30 * 24 * 60 * 60;
        if (age.Contains("jour")) return long.Parse(age.GetOnlyNumeric()) * 24 * 60 * 60;
        if (age.Contains("heure")) return long.Parse(age.GetOnlyNumeric()) * 60 * 60;
        if (age.Contains("minute")) return long.Parse(age.GetOnlyNumeric()) * 60;

        return long.Parse(age.GetOnlyNumeric());
    }
}