namespace Rong.Core.Models;

public class JwtSettings
{
    public string Secret { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int AccessExpiration { get; init; }
    public int RefreshExpiration { get; init; }
}