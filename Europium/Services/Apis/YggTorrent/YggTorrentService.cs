using System.Globalization;
using Europium.Dtos;
using Europium.Helpers.Extensions;
using Europium.Repositories;

namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentService
{
    private readonly YggTorrentRepository _yggTorrentRepository;

    public YggTorrentService(YggTorrentRepository yggTorrentRepository)
    {
        _yggTorrentRepository = yggTorrentRepository;
    }
    
    public async Task<YggTorrentAccount> GetRatio()
    {
        var response = await _yggTorrentRepository.GetRatio();

        response = CleanResponse(response); // return " 19.01To -  18.03To  Ratio : 1.054"

        var up = ExtractUp(response);
        var down = ExtractDown(response);
        return new YggTorrentAccount
        {
            Ratio = ExtractRatio(response),
            Up = up.Value,
            UpUnit = up.Unit,
            Down = down.Value,
            DownUnit = down.Unit,
        };
    }

    private string CleanResponse(string response)
    {
        response = response.RemoveBefore("Mes recherches");
        response = response.RemoveAfter("Compte");
        response = response.RemoveAllBetween('<', '>');

        return response;
    }

    private decimal ExtractRatio(string response)
    {
        var startIndex = response.IndexOf("Ratio : ", StringComparison.Ordinal) + "Ratio : ".Length;
        var responseString = response.Substring(startIndex, response.Length - startIndex);
        
        return decimal.Parse(responseString, CultureInfo.InvariantCulture);
    }

    private (decimal Value, string Unit) ExtractUp(string response)
    {
        var responseString = response.Split('-')[0];
        
        return (decimal.Parse(responseString.GetOnlyNumeric(), CultureInfo.InvariantCulture), GetUnit(responseString));
    }

    private (decimal Value, string Unit) ExtractDown(string response)
    {
        var responseString = response.Split('-')[1];
        responseString = responseString.RemoveAfter("Ratio");
        
        return (decimal.Parse(responseString.GetOnlyNumeric(), CultureInfo.InvariantCulture), GetUnit(responseString));
    }

    private string GetUnit(string response)
    {
        var responseTrimmed = response.TrimEnd();
        return responseTrimmed.Substring(responseTrimmed.Length - 2);
    }
}