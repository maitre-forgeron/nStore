using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using NStore.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace NStore.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Events.OnSigningOut = async e =>
            {
                await e.HttpContext.RevokeUserRefreshTokenAsync();
            };
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = configuration["Auth:Authority"];

            options.ClientId = configuration["Auth:ClientId"];
            options.ClientSecret = configuration["Auth:ClientSecret"];
            options.ResponseType = AuthenticationConstants.HybridCodeIdTokenResponseType;

            var scopes = configuration["Auth:Scopes"].Split(',').ToList();

            scopes.ForEach(scope => options.Scope.Add(scope));

            options.ClaimActions.MapJsonKey("role", "role", "role");
            options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;

            options.SaveTokens = true;

            options.GetClaimsFromUserInfoEndpoint = true;
        });

        services.AddAccessTokenManagement().ConfigureBackchannelHttpClient();

        return services;
    }
}
