using Microsoft.AspNetCore.Authentication;
using OAuth2.Client;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// var kakaoOAuth2Config = builder.Configuration.GetValue<OAuth2ClientConfiguration>("OAuth2:Provider:Kakao");
var kakaoOAuth2Config = new OAuth2ClientConfiguration();
builder.Configuration.GetSection("OAuth2:Provider:Kakao").Bind(kakaoOAuth2Config);

Console.WriteLine(kakaoOAuth2Config);
builder.Services.AddAuthentication()
    .AddOAuth("Kakao", "Kakao", o =>
        {
            o.ClientId = kakaoOAuth2Config.ClientId;
            o.ClientSecret = kakaoOAuth2Config.ClientSecret;
            o.CallbackPath = kakaoOAuth2Config.RedirectUri;
            o.AuthorizationEndpoint = kakaoOAuth2Config.AuthorizationUri;
            o.TokenEndpoint = kakaoOAuth2Config.TokenUri;
            o.UserInformationEndpoint = kakaoOAuth2Config.UserInfoUri;
            o.ClaimsIssuer = kakaoOAuth2Config.ClientName;
            o.SaveTokens = true;
            kakaoOAuth2Config.Scope.ToList()
                .ForEach(x => o.Scope.Add(x));
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment is { EnvironmentName: "Local" })
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();