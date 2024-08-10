using NStore.GatewayApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
