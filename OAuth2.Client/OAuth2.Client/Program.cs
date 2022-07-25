using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OAuth2.Client;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var kakaoOAuth2Config = new OAuth2ClientConfiguration();
builder.Configuration.GetSection("OAuth2:Provider:Kakao").Bind(kakaoOAuth2Config);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "kakao";
    })
    .AddCookie()
    .AddOAuth("kakao", "kakao", options =>
        {
            options.ClientId = kakaoOAuth2Config.ClientId;
            options.ClientSecret = kakaoOAuth2Config.ClientSecret;
            options.CallbackPath = new PathString(kakaoOAuth2Config.RedirectUri);
            // options.CallbackPath = new PathString("/signin-kakao");
            options.SaveTokens = true;
            options.AuthorizationEndpoint = kakaoOAuth2Config.AuthorizationUri;
            options.TokenEndpoint = kakaoOAuth2Config.TokenUri;
            options.UserInformationEndpoint = kakaoOAuth2Config.UserInfoUri;
            options.ClaimsIssuer = kakaoOAuth2Config.ClientName;
            options.SaveTokens = true;
            
            // kakaoOAuth2Config.Scope.ForEach(x => options.Scope.Add(x));
            
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "profile_nickname");
            options.ClaimActions.MapJsonKey("account_email", "account_email");
            options.ClaimActions.MapJsonKey("birthday", "birthday");

            // options.SignInScheme = ;

            // options.Events = new OAuthEvents
            // {
            //     OnCreatingTicket = context =>
            //     {
            //         var accessToken = context.AccessToken;
            //         var base64Payload = accessToken.Split('.')[1];
            //         var bytes = Convert.FromBase64String(base64Payload);
            //         var jsonPayload = Encoding.UTF8.GetString(bytes);
            //         var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);
            //
            //         // claims.Keys.ToList().ForEach(s => { context.Identity.AddClaim(new Claim(s, claims[s])); });
            //
            //         return Task.CompletedTask;
            //
            //         // var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            //         // request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //         // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            //         //
            //         // var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            //         // response.EnsureSuccessStatusCode();
            //         //
            //         // JObject user = JObject.Parse(await response.Content.ReadAsStringAsync());
            //         // context.RunClaimActions(user);
            //     }
            // };
            options.Events = new OAuthEvents()
            {
                OnTicketReceived = context =>
                {
                    Console.WriteLine(context);
                    return Task.CompletedTask;
                }
            };
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment is { EnvironmentName: "Local" }) {
    app.UseDeveloperExceptionPage();
}

app.Use((context, next) =>
{
    context.Request.Host = new HostString("localhost:8080");
    return next();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllers();

app.Run();