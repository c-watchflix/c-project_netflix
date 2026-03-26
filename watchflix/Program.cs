using watchflix.Components;
using watchflix.Repositories;
using watchflix.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Services application
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AdoFilm>();
builder.Services.AddScoped<AdoArtiste>();
builder.Services.AddScoped<AdoCategorie>();
builder.Services.AddScoped<AdoUser>();
builder.Services.AddScoped<AdoMusique>();
builder.Services.AddScoped<Authentification>();

builder.Services.AddAuthorizationCore();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();