using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace Zemoga_Test.Infrastructure;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),                
        };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
            {
                new ApiResource("ZemogaApi","Zemoga API Test")
                {
                    UserClaims = new[] { JwtClaimTypes.Name, JwtClaimTypes.Role, "zemoga" }
                }
            };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
            {
                new Client
                {
                    ClientId = "ZemogaWeb",
                    RequireClientSecret = false,
                    //AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "openid", "profile", "email", "read", "write", "delete", "edit", "zemoga", "ZemogaApi", IdentityServerConstants.LocalApi.ScopeName }
                }
            };
    }

    public static List<TestUser> GetTestUsers()
    {
        return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "demo",
                    Password = "demo"
                }
            };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
    {
            new ApiScope(name:IdentityServerConstants.LocalApi.ScopeName,   displayName: "Local Api"),
            new ApiScope(name: "ZemogaApi",   displayName: "Read your data."),
            new ApiScope(name: "zemoga",   displayName: "Read your data."),
            new ApiScope(name: "read",   displayName: "Read your data."),
            new ApiScope(name: "write",  displayName: "Write your data."),
            new ApiScope(name: "edit",  displayName: "Edit your data."),
            new ApiScope(name: "delete", displayName: "Delete your data.")
    };
    }
}