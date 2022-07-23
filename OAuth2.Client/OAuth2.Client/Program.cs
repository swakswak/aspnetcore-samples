using Microsoft.AspNetCore.Authentication;
using OAuth2.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OAuth2ClientConfiguration kakaoOauth2Configuration = builder.Configuration.GetSection("OAuth2:Provider:Kakao");
var environment = builder.Environment.EnvironmentName;
var kakaoOAuth2Config = builder.Configuration.GetSection("OAuth2:Provider:Kakao").Get<OAuth2ClientConfiguration>();
// builder.Services.Configure<OAuth2ClientConfiguration>(o =>
//     {
//         o.ClientId = builder.Configuration[""]
//     })
//     
//     .AddJsonFile($"appsettings.{environment}.json")
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();