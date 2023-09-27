using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Infrastructure.Services;

public class JwtService : IJwtService<User>
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;

    public JwtService(UserManager<User> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtSettings = jwtSettings;
    }

    public async Task<string> CreateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        var rolesBuilder = new StringBuilder();
        foreach (var role in userRoles)
        {
            rolesBuilder.Append(role);
            rolesBuilder.Append(',');
        }

        var roles = rolesBuilder.ToString().TrimEnd(',');
        claims.Add(new Claim(ClaimTypes.Role, roles));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.InvariantCulture)));

        var result = GenerateEncryptedToken(GetSigningCertificate(), claims);
        return result;
    }

    private string GenerateEncryptedToken(SigningCredentials credentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken
        (
            _jwtSettings.ValidIssuer,
            _jwtSettings.ValidAudience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Expiry),
            signingCredentials: credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SigningCredentials GetSigningCertificate()
    {
        var secret = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var key = new SymmetricSecurityKey(secret);
        var result = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return result;
    }
}