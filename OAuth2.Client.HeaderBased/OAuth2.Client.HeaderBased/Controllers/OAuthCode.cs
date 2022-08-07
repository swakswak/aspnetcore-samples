namespace OAuth2.Client.HeaderBased.Controllers;

public record OAuthCode
{
    public string Code { get; init; } = null!;
    public string? State { get; init; }
}