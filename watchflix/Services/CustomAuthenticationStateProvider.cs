using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace watchflix.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Appel à un endpoint pour vérifier l'état d'authentification du serveur
                var response = await _httpClient.GetAsync("https://localhost:7275/api/auth/state");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, content)
                        }, "Cookies");
                        var user = new ClaimsPrincipal(identity);
                        return new AuthenticationState(user);
                    }
                }
            }
            catch
            {
                // En cas d'erreur, retourner anonyme
            }

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}