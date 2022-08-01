using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class SonarrService : CommonApiService
{
	public SonarrService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		var monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.SONARR);
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", monitoredApi?.ApiKey);
	}
}