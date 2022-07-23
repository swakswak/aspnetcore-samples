using OAuth2.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OAuth2ClientConfiguration kakaoOauth2Configuration = builder.Configuration.GetSection("OAuth2:Provider:Kakao");
var environment = builder.Environment.EnvironmentName;

// builder.Services.Configure<OAuth2ClientConfiguration>(o =>
//     {
//         o.ClientId = builder.Configuration[""]
//     })
//     
//     .AddJsonFile($"appsettings.{environment}.json")
// builder.Services.AddAuthentication()
//     .AddOAuth("Kakao", "Kakao", o =>
//         o.ClientId = builder.Configuration.GetSection("Oauth2")
//     );

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