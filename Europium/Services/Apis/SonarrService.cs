using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class SonarrService : CommonApiService
{
	public SonarrService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.SONARR);
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
	}
}