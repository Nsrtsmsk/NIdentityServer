using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
    {
        opts.Authority = "https://localhost:44383";
        opts.Audience = "resource_api1";

    });
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("ReadPolicy", policy => { policy.RequireClaim("scope", "api1.read"); });
    opts.AddPolicy("UpdateOrCreate", policy => { policy.RequireClaim("scope", new[] { "api1.update","api1.create" }); });

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Kimlik Doðrulama
app.UseAuthorization(); //Yetkilendirme

app.MapControllers();

app.Run();
