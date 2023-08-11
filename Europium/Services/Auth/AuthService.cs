using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Europium.Models;
using Europium.Repositories.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthConfig = Europium.Models.AuthConfig;

namespace Europium.Services.Auth;

public class AuthService
{
    private readonly ConfigurationSettingRepository _configurationSettingRepository;
    private readonly AuthConfig _authConfig;

    public AuthService(ConfigurationSettingRepository configurationSettingRepository, IOptions<AppConfig> options)
    {
        _configurationSettingRepository = configurationSettingRepository;
        _authConfig = options.Value.AuthConfig;
    }

    public async Task<string?> Login(string user, string password)
    {
        if (user == await _configurationSettingRepository.GetConfigurationValue(Repositories.Models.AuthConfig.User) &&
            password == await _configurationSettingRepository.GetConfigurationValue(Repositories.Models.AuthConfig.Password))
        {
            return GenerateJwtToken(user);
        }

        return null;
    }
    
    public string GenerateJwtToken(string user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_authConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _authConfig.Issuer,
            Audience = _authConfig.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}