﻿using CartingService.Api.EventBusConsumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NStore.Shared.Constants;
using RabbitMQ.Client;

namespace CartingService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(configuration.GetSection("EventBusSettings:HostAddress").Value!),
                AutomaticRecoveryEnabled = true
            };

            return factory.CreateConnection();
        })
        .AddHealthChecks()
        .AddRabbitMQ();

        return services;
    }

    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumer<UpdatedProductsConsumer, UpdatedProductsConsumerDefinition>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetSection("EventBusSettings:HostAddress").Value);
            });
        });

        return services;
    }

    public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(AuthenticationConstants.AuthenticationSchemeBearer)
        .AddJwtBearer(AuthenticationConstants.AuthenticationSchemeBearer, options =>
        {
            options.Authority = configuration["Auth:Authority"];

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                RoleClaimType = AuthenticationConstants.RoleClaim,
                ValidIssuer = configuration["Auth:ValidIssuer"],
                SignatureValidator = (token, _) => new JsonWebToken(token)
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    // Log the error for diagnostics
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(context.Exception, "Authentication failed.");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    // Log successful token validation
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Token validated successfully.");
                    return Task.CompletedTask;
                }
            };

            options.MapInboundClaims = false;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim(AuthenticationConstants.ClientIdClaim, configuration["Auth:ClientId"]));
        });

        return services;
    }
}
