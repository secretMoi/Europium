using AutoMapper;
using Europium.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Europium.Repositories;

public class ApisToMonitorRepository
{
	private readonly EuropiumContext _europiumContext;
	private readonly IMapper _mapper;

	public ApisToMonitorRepository(EuropiumContext europiumContext, IMapper mapper)
	{
		_europiumContext = europiumContext;
		_mapper = mapper;
	}
	
	private async Task<bool> SaveChangesAsync()
	{
		return await _europiumContext.SaveChangesAsync() >= 0;
	}

	public async Task<List<ApiToMonitor>> GetAllAsync()
	{
		return await _europiumContext.ApisToMonitor.ToListAsync();
	}

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _europiumContext.ApisToMonitor
			.Include(p => p.ApiUrls)
			.FirstOrDefaultAsync(api => api.Code.Equals(apiCode)) ?? null;
	}

	public ApiToMonitor? GetApiByCode(string apiCode)
	{
		return _europiumContext.ApisToMonitor.FirstOrDefault(api => api.Code.Equals(apiCode));
	}

	private async Task<ApiToMonitor?> GetApiByIdAsync(int id)
	{
		return await _europiumContext.ApisToMonitor
			.Include(p => p.ApiUrls)
			.FirstOrDefaultAsync(api => api.ApiToMonitorId == id);
	}
	
	public async Task<bool> SaveApiAsync(ApiToMonitor apiToMonitor)
	{
		var apiInDatabase = await GetApiByIdAsync(apiToMonitor.ApiToMonitorId);

		if (apiToMonitor.ApiToMonitorId != 0 && apiInDatabase is not null)
		{
			_mapper.Map(apiToMonitor, apiInDatabase);
			_europiumContext.ApisToMonitor.Update(apiInDatabase);
		}
		else
		{
			await _europiumContext.ApisToMonitor.AddAsync(apiToMonitor);
		}
		
		return await SaveChangesAsync();
	}
}