namespace Europium.Repositories.Models;

public class ConfigurationSetting
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}