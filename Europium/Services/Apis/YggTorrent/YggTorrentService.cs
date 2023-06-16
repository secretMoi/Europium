using System.Globalization;
using Europium.Dtos;
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
            Ratio = ExtractRatioFromResponse(response)
        };
    }

    private string CleanResponse(string response)
    {
        response = response.Substring(response.IndexOf("Mes recherches", StringComparison.Ordinal) + "Mes recherches".Length);
        response = response.Substring(0, response.IndexOf("Compte", StringComparison.Ordinal));
        
        while (response.Contains('<'))
        {
            int start = response.LastIndexOf('<');
            int end = response.IndexOf('>', start);
            response = response.Remove(start, end - start + 1);
        }

        return response;
    }

    private decimal ExtractRatioFromResponse(string response)
    {
        var startIndex = response.IndexOf("Ratio : ", StringComparison.Ordinal) + "Ratio : ".Length;
        var responseString = response.Substring(startIndex, response.Length - startIndex);
        
        return decimal.Parse(responseString, CultureInfo.InvariantCulture);
    }
}