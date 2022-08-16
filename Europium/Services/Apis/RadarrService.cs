using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class RadarrService : CommonApiService
{
	public RadarrService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.RADARR);
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
	}
}