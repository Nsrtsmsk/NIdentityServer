using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

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
                },
                new Client()
                {
                    ClientId="Client1-Mvc",
                    ClientName = "Client1-Mvc APP Uygulaması",
                    RequirePkce =false, // serverside uygulamalarda gecerli. Secret key Tarayıcıya yüklenmesin diye yapılır. 
					ClientSecrets =new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:44375/signin-oidc"}, //Token alma işlemini gerçekleştiren URL'dir.Client1 StartUp da Oidc diye belirttiğimiz için.
					PostLogoutRedirectUris=new List<string>{ "https://localhost:44375/signout-callback-oidc" },
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,
                        "api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "CountryAndCity",
                        "Roles"
                    },//Bu Client hangi izinlere sahip olacak onu belirliyoruz.
					AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, // 60 gün sonra tükenecek.
					RequireConsent=true  // onay ekranı xxx uygulaması senden xxx bilgilerini istiyor vs.
				}

            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId() //subId. Gereklidir.
				{

                },
                new IdentityResources.Profile() // claim bilgilerini tutar
				{

                },
                new IdentityResource()
                {
                    Name="CountryAndCity",
                    DisplayName="County and City",
                    Description="Kullanıcının ülke ve şehir bilgisi",
                    UserClaims = new[]{"country","city"}
                },
                new IdentityResource()
                {
                    Name="Roles",
                    DisplayName="Roles",
                    Description="Kullanıcı Rolleri",
                    UserClaims = new[]{"role"}
                }

            };

        }
        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()// username email olarak kullanılabilir
				{
                    SubjectId = "1",Username ="Nusret",Password = "12345",Claims = new List<Claim>
                    {
                        new Claim("given_name","Nusret"),
                        new Claim("family_name","Simsek"),
                        new Claim("country","Türkiye"),
                        new Claim("city","Istanbul"),
                        new Claim("role","admin")
                    }
                },

                new TestUser()// username email olarak kullanılabilir
				{
                    SubjectId = "2",Username ="Dilek",Password = "11111",Claims = new List<Claim>
                    {
                        new Claim("given_name","Dilek"),
                        new Claim("family_name","Simsek"),
                        new Claim("country","Türkiye"),
                        new Claim("city","Rize"),
                         new Claim("role","customer")

                    }
                }

            };
        }
    }
}
