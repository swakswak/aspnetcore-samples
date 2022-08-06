using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace OAuth2.Client.Cookie;

public class CustomTokenAuthenticationHandler : AuthenticationHandler<CustomAuthenticationSchemeOptions>
{
    public CustomTokenAuthenticationHandler(IOptionsMonitor<CustomAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }
    // protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    // {
    //     Console.WriteLine($"[HandleSignInAsync] accessToken={Context.Request.Cookies["accessToken"]}");
    //     if (user is null)
    //         throw new ArgumentException(nameof(user));
    //
    //     properties = properties ?? new AuthenticationProperties();
    //
    //     var cookieOptions = Options.Cookie.Build(Context);
    //     cookieOptions.Expires = null;
    //
    //     var signingInContext = new CookieSigningInContext
    //     (
    //         Context,
    //         Scheme,
    //         Options,
    //         user,
    //         properties,
    //         cookieOptions
    //     );
    //
    //     var issued = Clock.UtcNow;
    //     signingInContext.Properties.IssuedUtc = issued;
    //     signingInContext.Properties.ExpiresUtc = issued.Add(Options.ExpireTimeSpan);
    //
    //     var cookieValue = "swak.swak.swak";
    //
    //     Debug.Assert(Options.Cookie.Name != null, "Options.Cookie.Name != null");
    //     Options.CookieManager.AppendResponseCookie
    //     (
    //         Context,
    //         Options.Cookie.Name,
    //         cookieValue,
    //         signingInContext.CookieOptions
    //     );
    //
    //     var signedInContext = new CookieSignedInContext
    //     (
    //         Context,
    //         Scheme,
    //         signingInContext.Principal,
    //         signingInContext.Properties,
    //         Options
    //     );
    //
    //     await Events.SignedIn(signedInContext);
    //
    //     var shouldRedirect = Options.LoginPath.HasValue && OriginalPath == Options.LoginPath;
    // }
    // public CustomTokenAuthenticationHandler(IOptionsMonitor<CookieAuthenticationOptions> options,
    //     ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    // {
    // }
    //
    // protected new CookieAuthenticationEvents Events
    // {
    //     get => (CookieAuthenticationEvents)base.Events;
    //     set => base.Events = value;
    // }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Console.WriteLine($"[HandleAuthenticateAsync] accessToken={Context.Request.Cookies["accessToken"]}");
        if (Context.Request.Cookies[CustomAuthenticationSchemeOptions.TokenName] == "swak.swak.swak")
            return AuthenticateResult.Success(new AuthenticationTicket(
                new ClaimsPrincipal(
                    new[]
                    {
                        new ClaimsIdentity(
                            new[]
                            {
                                new Claim("Role", "User")
                            }
                        )
                    }),
                CustomAuthenticationSchemeOptions.AuthenticationScheme
            ));

        return AuthenticateResult.Fail("No principal");
    }
}