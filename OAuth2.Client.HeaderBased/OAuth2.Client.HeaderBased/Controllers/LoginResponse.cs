namespace OAuth2.Client.HeaderBased.Controllers;

public record LoginResponse
{
    public string Token { get; init; } = null!;
}