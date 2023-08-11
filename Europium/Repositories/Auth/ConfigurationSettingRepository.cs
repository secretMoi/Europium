using Microsoft.EntityFrameworkCore;

namespace Europium.Repositories.Auth;

public class ConfigurationSettingRepository
{
    private readonly EuropiumContext _dbContext;

    public ConfigurationSettingRepository(EuropiumContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetConfigurationValue(int id)
    {
        return (await _dbContext.ConfigurationSettings.FirstAsync(s => s.Id == id)).Value;
    }

    // public async Task SetConfigurationValue(string key, string value)
    // {
    //     var setting = await _dbContext.ConfigurationSettings.FirstOrDefaultAsync(s => s.Key == key);
    //     if (setting != null)
    //     {
    //         setting.Value = value;
    //     }
    //     else
    //     {
    //         _dbContext.ConfigurationSettings.Add(new ConfigurationSetting { Key = key, Value = value });
    //     }
    //     await _dbContext.SaveChangesAsync();
    // }
}