﻿using System.Net;
using Europium.Repositories;
using Europium.Repositories.Models;

namespace Europium.Services.Apis;

public class CommonApiService : BaseApiRepository
{
	protected readonly ApisToMonitorRepository _apisToMonitorRepository;

	protected ApiToMonitor? _monitoredApi;

	public CommonApiService(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		
		var cookies = new CookieContainer();
		var handler = new HttpClientHandler();
		handler.CookieContainer = cookies;
		HttpClient = new HttpClient(handler);
	}
	
	public virtual async Task<bool> IsUpAsync(string url)
	{
		try
		{
			var response = await HttpClient.GetAsync(url + "/api/v3/system/status", GetCancellationToken(5));
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}