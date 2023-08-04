using AutoMapper;
using IdentityServer.Auth.Mapping_Profile;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

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
                    var daaaa = WithProfile<MappingProfile>.Map<IdentityServer4.EntityFramework.Entities.Client>(client);
                    context.Clients.Add(daaaa);
                }
                context.SaveChanges();
            }
            if (!context.ApiResources.Any()) 
            {
                foreach (var apiresource in Config.GetAPIResource())
                {
                    context.ApiResources.Add(WithProfile<MappingProfile>.Map<IdentityServer4.EntityFramework.Entities.ApiResource>(apiresource));
                }
                context.SaveChanges();
            }
            if (!context.ApiScopes.Any())
            {
                Config.GetAPIScopes().ToList().ForEach(s => 
                { 
                    context.ApiScopes.Add(WithProfile<MappingProfile>.Map<IdentityServer4.EntityFramework.Entities.ApiScope>(s)); 
                });
                context.SaveChanges();
            }
            if (!context.IdentityResources.Any())
            {
                Config.GetIdentityResources().ToList().ForEach(s =>
                {
                    context.IdentityResources.Add(WithProfile<MappingProfile>.Map<IdentityServer4.EntityFramework.Entities.IdentityResource>(s));
                });
                context.SaveChanges();
            }
        }
    }
}
