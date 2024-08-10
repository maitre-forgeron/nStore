using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NStore.Shared.Constants;

namespace NStore.GatewayApi;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("authenticated", policy => policy.RequireAuthenticatedUser());
        });

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

        return services;
    }
}
