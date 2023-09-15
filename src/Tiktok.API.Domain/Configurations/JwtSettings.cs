namespace Tiktok.API.Domain.Configurations;

public class JwtSettings
{
    public string SecretKey  { get; set; } = String.Empty;
    public string Issuer     { get; set; } = String.Empty;
    public string Audience   { get; set; } = String.Empty;
    /// <summary>
    /// using minutes
    /// </summary>
    public long Expiry { get; set; }
}