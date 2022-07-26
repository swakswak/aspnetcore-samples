namespace OAuth2.Client.Security;

public class OAuth2ClientConfiguration
{
    public string AuthenticationScheme { get; set; }

    public string DisplayName { get; set; }
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
    public string AuthorizationUri { get; set; }
    public string TokenUri { get; set; }
    public string UserInfoUri { get; set; }
    public string UserInfoAuthenticationMethod { get; set; }
    public string UserNameAttribute { get; set; }
    public Scope Scope { get; set; }
}