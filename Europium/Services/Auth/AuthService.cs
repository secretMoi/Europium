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
    private readonly RefreshTokenRepository _refreshTokenRepository;
    private readonly AuthConfig _authConfig;

    public AuthService(ConfigurationSettingRepository configurationSettingRepository, IOptions<AppConfig> options, RefreshTokenRepository refreshTokenRepository)
    {
        _configurationSettingRepository = configurationSettingRepository;
        _refreshTokenRepository = refreshTokenRepository;
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
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _authConfig.Issuer,
            Audience = _authConfig.Audience,
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<string> GenerateRefreshToken(string userName)
    {
        // var tokenHandler = new JwtSecurityTokenHandler();
        // var key = Encoding.ASCII.GetBytes(_authConfig.Key);

        var refreshToken = new RefreshToken
        {
            UserName = userName,
            Token = GenerateJwtToken(userName),
            ExpiryDate = DateTime.UtcNow.AddMonths(1)
        };

        await _refreshTokenRepository.Remove(userName);
        await _refreshTokenRepository.Add(refreshToken);

        return refreshToken.Token;
    }

    public (ClaimsPrincipal, string) ValidateRefreshToken(string token)
    {
        var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _authConfig.Audience,
            ValidIssuer = _authConfig.Issuer,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        // Check if the token is a valid refresh token
        if (!_refreshTokenRepository.CheckIfTokenIsValid(token))
            return (null, null);

        return (principal, ((JwtSecurityToken)validatedToken).RawData);
    }
}