using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer.Auth.Seeds
{
    public static class SeedData
    {
        public static void Seed(ConfigurationDbContext context)
        { 
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients()) 
                {
                    context.Clients.Add(client.ToEntity());
                
                }
            }
            if (!context.ApiResources.Any()) 
            {
                foreach (var apiresource in Config.GetAPIResource())
                {
                    context.ApiResources.Add(apiresource.ToEntity());
                }
            }
            if (!context.ApiScopes.Any())
            {
                Config.GetAPIScopes().ToList().ForEach(s => 
                { 
                    context.ApiScopes.Add(s.ToEntity()); 
                });
            }
            if (!context.IdentityResources.Any())
            {
                Config.GetIdentityResources().ToList().ForEach(s =>
                {
                    context.IdentityResources.Add(s.ToEntity());
                });
            }

            context.SaveChanges();
        }
    }
}
