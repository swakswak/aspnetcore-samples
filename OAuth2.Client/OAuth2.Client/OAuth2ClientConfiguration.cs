namespace OAuth2.Client;

public class OAuth2ClientConfiguration
{
    public string ClientId { get; init; }
    public string ClientName { get; init; }
    public string ClientSecret { get; init; }
    public string RedirectUri { get; init; }
    public string AuthorizationUri { get; init; }
    public string TokenUri { get; init; }
    public string UserInfoUri { get; init; }
    public string UserInfoAuthenticationMethod { get; init; }
    public string UserNameAttribute { get; init; }
    public IList<string> Scope { get; init; }
}