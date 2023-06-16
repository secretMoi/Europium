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

        response = CleanResponse(response);

        return new YggTorrentAccount
        {
            Ratio = ExtractRatio(response),
            Up = ExtractUp(response),
            Down = ExtractDown(response)
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

    private decimal ExtractUp(string response)
    {
        var responseString = response.Split('-')[0];
        responseString = responseString.GetOnlyNumeric();
        
        return decimal.Parse(responseString, CultureInfo.InvariantCulture);
    }

    private decimal ExtractDown(string response)
    {
        var responseString = response.Split('-')[1];
        responseString = responseString.RemoveAfter("Ratio");
        responseString = responseString.GetOnlyNumeric();
        
        return decimal.Parse(responseString, CultureInfo.InvariantCulture);
    }
}