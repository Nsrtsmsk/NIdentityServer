using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddHttpContextAccessor(); // IAPIRESOURCEHTTPACCESSOR ÝÇÝN.
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme = "Cookies";
    opts.DefaultScheme = "oidc";
})
    .AddCookie
    (
    "Cookies",
    opts=>opts.AccessDeniedPath = "/Home/AccessDenied"
    )
    .AddOpenIdConnect("oidc", opts =>
{
    opts.SignInScheme = "Cookies";
    opts.Authority = "https://localhost:44383";
    opts.ClientId = "Client1-Mvc";
    opts.ClientSecret = "secret";
    opts.ResponseType = "code id_token";
    opts.GetClaimsFromUserInfoEndpoint = true; // userýnfo endpointinden gelen bilgileri set eder.
    opts.SaveTokens = true; // access tokenlarý kaydeder.
    opts.Scope.Add("api1.read");// api.read scope'unu da ver bana.
    opts.Scope.Add("offline_access");// RefreshToken istiyoruz.
    opts.Scope.Add("CountryAndCity");// manuel eklediðimiz scope u istiyoruz.
    opts.Scope.Add("Roles");// manuel eklediðimiz scope u istiyoruz.


    opts.ClaimActions.MapUniqueJsonKey("country", "country");
    opts.ClaimActions.MapUniqueJsonKey("city", "city");
    opts.ClaimActions.MapUniqueJsonKey("role","role");
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role"
    };
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
