using watchflix.Components;
using watchflix.Repositories;
using Microsoft.EntityFrameworkCore;
using watchflix.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AdoFilm>();
builder.Services.AddScoped<AdoArtiste>();
builder.Services.AddScoped<AdoCategorie>();
builder.Services.AddScoped<AdoUser>();
builder.Services.AddScoped<AdoMusique>();
builder.Services.AddScoped<YoutubeServiceOfficial>(provider => new YoutubeServiceOfficial("AIzaSyAVze5iCrB-gvjas-GKwoa5TVJ2EsESfUo"));
builder.Services.AddScoped<YoutubeServiceOfficial2>(provider => new YoutubeServiceOfficial2("AIzaSyAVze5iCrB-gvjas-GKwoa5TVJ2EsESfUo"));
builder.Services.AddScoped<watchflix.Services.TmdbService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<Authentification>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();