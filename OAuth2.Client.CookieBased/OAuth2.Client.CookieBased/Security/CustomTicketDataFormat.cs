using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace OAuth2.Client.Security;

public class CustomTicketDataFormat : ISecureDataFormat<AuthenticationTicket>

{
    public string Protect(AuthenticationTicket data)
    {
        return Protect(data, null);
    }

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        return "Role:User";
    }

    public AuthenticationTicket? Unprotect(string? protectedText)
    {
        return Unprotect(protectedText, null);
    }

    public AuthenticationTicket? Unprotect(string? protectedText, string? purpose)
    {
        if ("Role:User" != protectedText)
            return default;

        return new AuthenticationTicket(
            new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new[]
                {
                    new Claim("Role", "User")
                })
            }),
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}