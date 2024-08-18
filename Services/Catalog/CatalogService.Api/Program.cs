using Catalog.Api.Middlewares;
using CatalogService.Api.Extensions;
using CatalogService.Application;
using CatalogService.Infrastructure;
using CatalogService.Infrastructure.Db;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NStore.Logging;
using NStore.Shared.Constants;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddEndpoints(typeof(Program).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddMassTransitCustomConfiguration(builder.Configuration);

builder.Services.AddHealthChecksConfiguration(builder.Configuration);

builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<CatalogServicesDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//app.MapControllers();
app.MapEndpoints();

try
{
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, ErrorMessageConstants.UnexpectedErrorMessage);
}
