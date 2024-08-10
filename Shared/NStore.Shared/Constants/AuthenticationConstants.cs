namespace NStore.Shared.Constants;

public static class AuthenticationConstants
{
    public static string HybridCodeIdTokenResponseType => "code id_token";

    public static string AuthenticationSchemeBearer => "Bearer";

    public static string ClientIdClaim => "client_id";

    public static string RoleClaim => "role";

    public static string RoleManager => "Manager";

    public static string RoleBuyer => "Buyer";
}
