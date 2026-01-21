using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace watchflix.Models
{
    // --- Recherche ---
    public class TmdbSearchResponse
    {
        [JsonPropertyName("results")]
        public List<MovieResult> Results { get; set; } = new List<MovieResult>();
    }

    public class MovieResult
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; } = "";
        [JsonPropertyName("poster_path")] public string PosterPath { get; set; } = "";
        
        public string FullImageUrl => string.IsNullOrEmpty(PosterPath) 
            ? "https://via.placeholder.com/200x300?text=No+Image" 
            : $"https://image.tmdb.org/t/p/w500{PosterPath}";
    }

    // --- Détails Complets ---
    public class MovieDetails
    {
        [JsonPropertyName("id")] public int Id { get; set; } // Important pour la BDD
        [JsonPropertyName("title")] public string Title { get; set; } = "";
        [JsonPropertyName("overview")] public string Overview { get; set; } = "";
        [JsonPropertyName("release_date")] public string ReleaseDate { get; set; } = "";

        public string AnneeSortie => !string.IsNullOrEmpty(ReleaseDate) && ReleaseDate.Length >= 4 
            ? ReleaseDate.Substring(0, 4) : "N/A";

        [JsonPropertyName("runtime")] public int Runtime { get; set; } // Minutes

        [JsonPropertyName("poster_path")] public string PosterPath { get; set; } = "";
        [JsonPropertyName("backdrop_path")] public string BackdropPath { get; set; } = "";

        // On garde les URLs complètes pour les sauver en BDD si besoin
        public string FullPosterUrl => $"https://image.tmdb.org/t/p/w500{PosterPath}";
        public string FullBackdropUrl => $"https://image.tmdb.org/t/p/original{BackdropPath}";

        // --- Nouveautés : Genres et PEGI ---
        
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; } = new List<Genre>();

        [JsonPropertyName("release_dates")]
        public ReleaseDatesContainer ReleaseDates { get; set; } = new ReleaseDatesContainer();

        [JsonPropertyName("videos")]
        public VideoContainer Videos { get; set; } = new VideoContainer();

        [JsonPropertyName("credits")]
        public CreditsContainer Credits { get; set; } = new CreditsContainer();

        // --- Helpers ---
        public string? YoutubeKey => Videos.Results.FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer")?.Key;
        public string? Realisateur => Credits.Crew.FirstOrDefault(c => c.Job == "Director")?.Name;

        // Logique pour trouver le PEGI France (FR)
        public string Pegi 
        {
            get 
            {
                var france = ReleaseDates.Results.FirstOrDefault(r => r.Iso3166_1 == "FR");
                if (france != null)
                {
                    // On cherche une certification non vide
                    var cert = france.ReleaseDates.FirstOrDefault(r => !string.IsNullOrEmpty(r.Certification))?.Certification;
                    return !string.IsNullOrEmpty(cert) ? cert : "Non classifié";
                }
                return "Inconnu";
            }
        }
    }

    // --- Sous-classes techniques ---
    public class Genre { [JsonPropertyName("id")] public int Id { get; set; } [JsonPropertyName("name")] public string Name { get; set; } = ""; }
    
    // Structure complexe pour le PEGI
    public class ReleaseDatesContainer { [JsonPropertyName("results")] public List<ReleaseLocation> Results { get; set; } = new List<ReleaseLocation>(); }
    public class ReleaseLocation { [JsonPropertyName("iso_3166_1")] public string Iso3166_1 { get; set; } = ""; [JsonPropertyName("release_dates")] public List<ReleaseDateItem> ReleaseDates { get; set; } = new List<ReleaseDateItem>(); }
    public class ReleaseDateItem { [JsonPropertyName("certification")] public string Certification { get; set; } = ""; }

    public class VideoContainer { [JsonPropertyName("results")] public List<VideoResult> Results { get; set; } = new List<VideoResult>(); }
    public class VideoResult { [JsonPropertyName("key")] public string Key { get; set; } = ""; [JsonPropertyName("site")] public string Site { get; set; } = ""; [JsonPropertyName("type")] public string Type { get; set; } = ""; }

    public class CreditsContainer { [JsonPropertyName("crew")] public List<CrewResult> Crew { get; set; } = new List<CrewResult>(); }
    public class CrewResult { [JsonPropertyName("job")] public string Job { get; set; } = ""; [JsonPropertyName("name")] public string Name { get; set; } = ""; }
}