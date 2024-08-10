using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace NStore.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(name: "cartingApi", displayName: "Carting Api", new List<string> { "role" }),
                new ApiScope(name: "catalogApi", displayName: "Catalog Api", new List<string> { "role" })
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "storeWeb",
                    ClientName = "Store Web",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    AllowOfflineAccess = true,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:7001/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:7001/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "cartingApi",
                        "catalogApi",
                    }
                }
            };
    }
}
