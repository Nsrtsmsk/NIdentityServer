using IdentityServer4.Models;

namespace IdentityServer.Auth
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetAPIResource()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                { Scopes =
                    {
                        "api1.read",
                        "api1.write",
                        "api1.update"
                    },
                    ApiSecrets = new[]{new Secret("secretapi1".Sha256())} // passpoword and username for basic auth
                },
                new ApiResource("resource_api2") 
                { Scopes =
                    {
                        "api2.read",
                        "api2.write",
                        "api2.update"
                    },
                    ApiSecrets = new[]{new Secret("secretapi2".Sha256())} // passpoword and username for basic auth
                }
            };
        }
        public static IEnumerable<ApiScope> GetAPIScopes()
        {
            return new List<ApiScope>() 
            { new ApiScope("api1.read", "Read permission for api1"),
              new ApiScope("api1.write","Write permission for api1"),
              new ApiScope("api1.update","Update permission for api1"),
              new ApiScope("api2.read", "Read permission for api2"),
              new ApiScope("api2.write","Write permission for api2"),
              new ApiScope("api2.update","Update permission for api2")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="client1",
                    ClientName = "Client 1 APP Uygulaması",
                    ClientSecrets =new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read", "api1.write", "api1.update" }
                }, 
                new Client()
                {
                    ClientId="client2",
                    ClientName = "Client 2 APP Uygulaması",
                    ClientSecrets =new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api2.read", "api2.write", "api2.update" }
                }
            };
        }
    }
}
