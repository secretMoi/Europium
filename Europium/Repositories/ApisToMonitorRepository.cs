using Europium.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Europium.Repositories;

public class ApisToMonitorRepository
{
	private readonly EuropiumContext _europiumContext;

	public ApisToMonitorRepository(EuropiumContext europiumContext)
	{
		_europiumContext = europiumContext;
	}
	
	public async Task<bool> SaveChangesAsync()
	{
		// permet d'appliquer les modifications à la db
		return await _europiumContext.SaveChangesAsync() >= 0;
	}

	public async Task<List<ApiToMonitor?>> GetAllAsync()
	{
		return await _europiumContext.ApisToMonitor.ToListAsync();
	}

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _europiumContext.ApisToMonitor.Include(p => p.ApiUrls).FirstOrDefaultAsync(api => api != null && api.Code.Equals(apiCode));
	}

	public ApiToMonitor? GetApiByCode(string apiCode)
	{
		return _europiumContext.ApisToMonitor.FirstOrDefault(api => api != null && api.Code.Equals(apiCode));
	}
}