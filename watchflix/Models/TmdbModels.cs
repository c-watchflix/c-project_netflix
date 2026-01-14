using System.Collections.Generic;
using System.Linq; // Important pour les filtres (Réalisateur, Youtube)
using System.Text.Json.Serialization;

namespace watchflix.Models
{
    // --- Pour la RECHERCHE (La liste) ---
    public class TmdbSearchResponse
    {
        [JsonPropertyName("results")]
        public List<MovieResult> Results { get; set; } = new List<MovieResult>();
    }

    public class MovieResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = "";
        
        public string FullImageUrl => string.IsNullOrEmpty(PosterPath) 
            ? "https://via.placeholder.com/200x300?text=No+Image" 
            : $"https://image.tmdb.org/t/p/w500{PosterPath}";
    }

    // --- Pour les DÉTAILS (La fiche complète) ---
    public class MovieDetails
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = "";

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = "";

        public string AnneeSortie => !string.IsNullOrEmpty(ReleaseDate) && ReleaseDate.Length >= 4 
            ? ReleaseDate.Substring(0, 4) 
            : "N/A";

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; } // Durée en minutes

        // Images
        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = "";
        
        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; } = "";

        public string FullPosterUrl => $"https://image.tmdb.org/t/p/w500{PosterPath}";
        public string FullBackdropUrl => $"https://image.tmdb.org/t/p/original{BackdropPath}";

        // --- Données complexes (Vidéo, Équipe, Age) ---
        
        [JsonPropertyName("videos")]
        public VideoContainer Videos { get; set; } = new VideoContainer();

        [JsonPropertyName("credits")]
        public CreditsContainer Credits { get; set; } = new CreditsContainer();

        // Helpers : Ce sont des raccourcis pour vous faciliter la vie
        
        // 1. Trouve la clé Youtube de la bande annonce
        public string? YoutubeKey => Videos.Results
            .FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer")?.Key;

        // 2. Trouve le réalisateur
        public string? Realisateur => Credits.Crew
            .FirstOrDefault(c => c.Job == "Director")?.Name;
    }

    // Structures internes pour lire le JSON complexe de TMDB
    public class VideoContainer { [JsonPropertyName("results")] public List<VideoResult> Results { get; set; } = new List<VideoResult>(); }
    public class VideoResult { [JsonPropertyName("key")] public string Key { get; set; } = ""; [JsonPropertyName("site")] public string Site { get; set; } = ""; [JsonPropertyName("type")] public string Type { get; set; } = ""; }

    public class CreditsContainer { [JsonPropertyName("crew")] public List<CrewResult> Crew { get; set; } = new List<CrewResult>(); }
    public class CrewResult { [JsonPropertyName("job")] public string Job { get; set; } = ""; [JsonPropertyName("name")] public string Name { get; set; } = ""; }
}