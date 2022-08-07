using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OAuth2.Client;
using OAuth2.Client.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    options.HttpsPort = 8081;
});

var kakao = new OAuth2ClientConfiguration();
builder.Configuration.GetSection("OAuth2:Provider:Kakao").Bind(kakao);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = kakao.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.TicketDataFormat = new CustomTicketDataFormat();
        options.Cookie.MaxAge = TimeSpan.FromMinutes(60);
    })
    .AddOAuth(kakao.AuthenticationScheme, kakao.DisplayName, options =>
    {
        options.ClientId = kakao.ClientId;
        options.ClientSecret = kakao.ClientSecret;
        options.CallbackPath = new PathString(kakao.RedirectUri);
        options.AuthorizationEndpoint = kakao.AuthorizationUri;
        options.TokenEndpoint = kakao.TokenUri;
        options.UserInformationEndpoint = kakao.UserInfoUri;
        options.ClaimsIssuer = kakao.ClientName;
        options.SaveTokens = true;

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, kakao.Scope.NameIdentifier);
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, kakao.Scope.Name);
        kakao.Scope.Others.ForEach(s => options.ClaimActions.MapJsonKey(s, s));

        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization(options =>
{
    var authorizationPolicy = new AuthorizationPolicy(
        new[]
        {
            new ClaimsAuthorizationRequirement("Role", new[] { "User" })
        },
        new[]
        {
            kakao.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme
        }
    );

    options.DefaultPolicy = authorizationPolicy;
});

var app = builder.Build();

if (app.Environment.EnvironmentName is "Development" or "Local" ) app.UseDeveloperExceptionPage();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();