using IdentityServer.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Config.GetAPIResource())
    .AddInMemoryApiScopes(Config.GetAPIScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddTestUsers(Config.GetUsers().ToList())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddDeveloperSigningCredential();
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

app.Run();
