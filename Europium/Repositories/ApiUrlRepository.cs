using Europium.Repositories.Models;

namespace Europium.Repositories;

public class ApiUrlRepository
{
	private readonly EuropiumContext _europiumContext;

	public ApiUrlRepository(EuropiumContext europiumContext)
	{
		_europiumContext = europiumContext;
	}
	
	public async Task<bool> SaveChangesAsync()
	{
		// permet d'appliquer les modifications à la db
		return await _europiumContext.SaveChangesAsync() >= 0;
	}

	public async Task<bool> AddApiUrlAsync(ApiUrl apiUrl)
	{
		await _europiumContext.ApiUrls.AddAsync(apiUrl);
		return await SaveChangesAsync();
	}
}