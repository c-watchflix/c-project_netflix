using RestSharp;
using System.Threading.Tasks;

namespace watchflix.Services // Le nom de notre projet
{
    public class TmdbService
    {
        private readonly string _token;
        private readonly RestClient _client;

        public TmdbService()
        {
            // Notre Token d'authentification TMDB
            _token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0ODQ3MzZjNjJiYWZmY2JiYmUxODMyM2UxNGJmMTk1OCIsIm5iZiI6MTc2ODM3Njc2NS4zNDcwMDAxLCJzdWIiOiI2OTY3NDliZDNhZTg1ZDk0YTc4NjYxYTAiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.Zz7ro2ZILWjXQSfoA3ZF1vzSJdrYXw1FFuzo_OxEoBM";
            
            var options = new RestClientOptions("https://api.themoviedb.org/3");
            _client = new RestClient(options);
        }

        // Cette fonction sert à chercher un film par son nom
        public async Task<string> SearchMovieByName(string movieName)
        {
            // Demande de recherche
            var request = new RestRequest("search/movie");
            
            // Authentification
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("accept", "application/json");

            // Paramètres : le nom du film et la langue
            request.AddQueryParameter("query", movieName);
            request.AddQueryParameter("language", "fr-FR");

            // Envoi
            var response = await _client.GetAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content; // On retourne le texte brut pour l'instant
            }
            else
            {
                return "Erreur : " + response.ErrorMessage;
            }
        }
    }
}