using IdentityServer.Auth;
using IdentityServer.Auth.Seeds;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// **********entityframework implementation.************
var assembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

//***********************
//builder.Services.AddIdentityServer()
//    .AddInMemoryApiResources(Config.GetAPIResource())
//    .AddInMemoryApiScopes(Config.GetAPIScopes())
//    .AddInMemoryClients(Config.GetClients())
//    .AddTestUsers(Config.GetUsers().ToList())
//    .AddInMemoryIdentityResources(Config.GetIdentityResources())
//    .AddDeveloperSigningCredential();
builder.Services.AddIdentityServer()
    .AddConfigurationStore(opts =>
    {
        var connectionString = builder.Configuration["LocalDb:myDb1"];
        opts.ConfigureDbContext = c => c.UseSqlServer(connectionString, sqlopt => sqlopt.MigrationsAssembly(assembly));
        
    })
    .AddOperationalStore
    (
    opts =>
    {
        var connectionString = builder.Configuration["LocalDb:myDb1"];
        opts.ConfigureDbContext = c => c.UseSqlServer(connectionString, sqlopt => sqlopt.MigrationsAssembly(assembly));

    }
    );
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    using (var serviceScope = app.Services.CreateScope())
    {
        var servis = serviceScope.ServiceProvider;
        var context = servis.GetRequiredService<ConfigurationDbContext>();
        SeedData.Seed(context);
    }

    app.Run();
