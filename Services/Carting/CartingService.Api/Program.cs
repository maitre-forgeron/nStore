using Asp.Versioning;
using CartingService.Api.Extensions;
using CartingService.Api.Infrastructure.Middlewares;
using CartingService.Application.Carts.Commands;
using CartingService.BLL.Carting;
using CartingService.DAL.Carting;
using CartingService.DAL.Db;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using NStore.Logging;
using NStore.Shared.Constants;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "API V2", Version = "v2" });

    options.DocInclusionPredicate((version, desc) =>
    {
        var v = desc.GetApiVersion();

        return version == $"v{v.MajorVersion}";
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(MongoDbSettings.MongoDbSettingsName));

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var conection = builder.Configuration.GetConnectionString("CartingDb");

    CartMapping.Map();
    
    return new MongoClient(conection);
});

builder.Services.AddScoped<CartDbContext>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(AddItemToCartCommand))!));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransitConfiguration(builder.Configuration);

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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
    });
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

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, ErrorMessageConstants.UnexpectedErrorMessage);
}
