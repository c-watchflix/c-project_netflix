using RestSharp;
using System.Threading.Tasks;
using watchflix.Models;

namespace watchflix.Services
{
    public class TmdbService
    {
        private readonly string _token;
        private readonly RestClient _client;

        public TmdbService()
        {
            // Votre Token
            _token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0ODQ3MzZjNjJiYWZmY2JiYmUxODMyM2UxNGJmMTk1OCIsIm5iZiI6MTc2ODM3Njc2NS4zNDcwMDAxLCJzdWIiOiI2OTY3NDliZDNhZTg1ZDk0YTc4NjYxYTAiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.Zz7ro2ZILWjXQSfoA3ZF1vzSJdrYXw1FFuzo_OxEoBM";
            var options = new RestClientOptions("https://api.themoviedb.org/3");
            _client = new RestClient(options);
        }

        public async Task<TmdbSearchResponse?> SearchMovieByName(string movieName)
        {
            var request = new RestRequest("search/movie");
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddQueryParameter("query", movieName);
            request.AddQueryParameter("language", "fr-FR");
            return await _client.GetAsync<TmdbSearchResponse>(request);
        }

        // --- NOUVELLE FONCTION POUR LES DÉTAILS ---
        public async Task<MovieDetails?> GetMovieDetails(int movieId)
        {
            // On demande le film par son ID
            // On ajoute "videos" et "credits" (équipe) dans la demande
            var request = new RestRequest($"movie/{movieId}");
            
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddQueryParameter("language", "fr-FR");
            request.AddQueryParameter("append_to_response", "videos,credits"); // La magie est ici

            return await _client.GetAsync<MovieDetails>(request);
        }
    }
}