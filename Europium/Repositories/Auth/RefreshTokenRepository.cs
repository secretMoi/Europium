using Europium.Models;

namespace Europium.Repositories.Auth;

public class RefreshTokenRepository
{
	private readonly EuropiumContext _dbContext;

	public RefreshTokenRepository(EuropiumContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(RefreshToken refreshToken)
	{
		_dbContext.RefreshTokens.Add(refreshToken);
		await _dbContext.SaveChangesAsync();
	}

	public async Task Remove(string userName)
	{
		var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(x => x.UserName == userName);
		if (refreshToken is null) return;
		
		_dbContext.RefreshTokens.Remove(refreshToken);
		await _dbContext.SaveChangesAsync();
	}

	public bool CheckIfTokenIsValid(string token)
	{
		return _dbContext.RefreshTokens.Any(rt => rt.Token == token && rt.ExpiryDate > DateTime.UtcNow);
	}
}