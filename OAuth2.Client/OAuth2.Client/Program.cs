using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using OAuth2.Client;
using OAuth2.Client.Cookie;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    options.HttpsPort = 8081;
});

var kakaoOAuth2Config = new OAuth2ClientConfiguration();
builder.Configuration.GetSection("OAuth2:Provider:Kakao").Bind(kakaoOAuth2Config);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "PolicyScheme";
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = kakaoOAuth2Config.AuthenticationScheme;
    })
    .AddScheme<CustomAuthenticationSchemeOptions, CustomTokenAuthenticationHandler>(
        CustomAuthenticationSchemeOptions.AuthenticationScheme, options => { })
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOAuth(kakaoOAuth2Config.AuthenticationScheme, kakaoOAuth2Config.DisplayName, options =>
        {
            options.ClientId = kakaoOAuth2Config.ClientId;
            options.ClientSecret = kakaoOAuth2Config.ClientSecret;
            options.CallbackPath = new PathString(kakaoOAuth2Config.RedirectUri);
            options.SaveTokens = true;
            options.AuthorizationEndpoint = kakaoOAuth2Config.AuthorizationUri;
            options.TokenEndpoint = kakaoOAuth2Config.TokenUri;
            options.UserInformationEndpoint = kakaoOAuth2Config.UserInfoUri;
            options.ClaimsIssuer = kakaoOAuth2Config.ClientName;
            options.SaveTokens = true;

            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "profile_nickname");
            options.ClaimActions.MapJsonKey("account_email", "account_email");

            options.CorrelationCookie.SameSite = SameSiteMode.None;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        }
    )
    // .AddPolicyScheme("PolicyScheme", "PolicyScheme", options =>
    // {
    //     options.ForwardDefaultSelector = context =>
    //     {
    //            
    //     }
    // })
    ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment is { EnvironmentName: "Local" }) app.UseDeveloperExceptionPage();

app.Use((context, next) =>
{
    context.Request.Host = new HostString("localhost:8080");
    return next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();