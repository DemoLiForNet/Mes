using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Mes.API.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("MesApi", "Mes Api",new string[]{JwtClaimTypes.Role })
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "post_man_client",
                    ClientName = "Post man client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RequireConsent = false,
                    AccessTokenLifetime = 3600 * 2,
                    ClientSecrets =new List<Secret>
                    {
                        new Secret("post_man_secret".Sha256())
                    },
                    AllowedScopes = new string[]
                    {   "MesApi"
                    },
                },
                new Client
                {
                    ClientId = "admin_client",
                    ClientName = "Admin Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { "http://localhost:4200/account/signin-callback" },
                    PostLogoutRedirectUris = { "http://localhost:4200/account/index" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    AccessTokenLifetime=3600 * 24 * 7,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MesApi"
                    },
                },
                       new Client
                {
                    ClientId = "test_client",
                    ClientName = "Test Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { "http://localhost:4200/account/signin-callback" },
                    PostLogoutRedirectUris = { "http://localhost:4200/account/index" },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    AccessTokenLifetime=3600 * 24 * 7,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MesApi"
                    }
                }
            };
        }
    }
}
