using Microsoft.AspNetCore.Authentication;

namespace OAuth2.Client.Cookie;

public class CustomAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string AuthenticationScheme = "CustomToken";
    public const string TokenName = AuthenticationScheme;
}