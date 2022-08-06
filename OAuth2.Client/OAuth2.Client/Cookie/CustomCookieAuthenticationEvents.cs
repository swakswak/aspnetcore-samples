using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace OAuth2.Client.Cookie;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{

    public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;
        var lastChanged = (from c in userPrincipal.Claims 
                                where c.Type == "LastChanged"
                                select c.Value).FirstOrDefault();

        if (string.IsNullOrEmpty(lastChanged))
        {
            context.RejectPrincipal();

            context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        return Task.CompletedTask;
    }
}