namespace Infrastructure.Configurations;


public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;

    /// <summary>
    ///     using minutes
    /// </summary>
    public long Expiry { get; set; }
}