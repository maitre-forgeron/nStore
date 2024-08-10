using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace NStore.Web.HttpHandlers;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthenticationDelegatingHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await LogTokenAndClaims();

        var accessToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.SetBearerToken(accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
    private async Task LogTokenAndClaims()
    {
        var identityToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

        Debug.WriteLine($"Identity token: {identityToken}");

        foreach (var claim in _contextAccessor.HttpContext.User.Claims)
        {
            Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
        }
    }
}
