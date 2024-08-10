using System.Security.Claims;

namespace NStore.Web.Extensions;

public static class AuthenticatedUserExtensions
{
    public static string GetAuthenticatedUserId(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
}
