using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using watchflix.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
public class YoutubeServiceOfficial2
{
    private readonly YouTubeService _youtube;
    public YoutubeServiceOfficial2(string apiKey)
    {
        _youtube = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "WatchFlix"
        });
    }

    public async Task<List<Musique>> SearchMusicAsync(string movieTitle)
    {
        // 1. Construire la requête (plus pertinente pour soundtrack)
        string query = $"{movieTitle} soundtrack complete";

        // 2. Recherche YouTube (1er appel API)
        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "video";
        searchRequest.VideoCategoryId = "10"; // Musique
        searchRequest.MaxResults = 10;

        var searchResponse = await searchRequest.ExecuteAsync();

        // Sécurité
        if (searchResponse.Items == null || searchResponse.Items.Count == 0)
            return new List<Musique>();

        // 3. Récupérer tous les IDs des vidéos
        var videoIds = searchResponse.Items
            .Where(i => i.Id.VideoId != null)
            .Select(i => i.Id.VideoId)
            .ToList();

        // 4. Requête groupée pour récupérer les durées (2e appel API)
        var videoRequest = _youtube.Videos.List("contentDetails,snippet");
        videoRequest.Id = string.Join(",", videoIds);

        var videoResponse = await videoRequest.ExecuteAsync();

        // 5. Transformer en objets Musique
        var musiques = videoResponse.Items.Select(item =>
        {
            var duration = System.Xml.XmlConvert.ToTimeSpan(item.ContentDetails.Duration);

            string url = $"https://www.youtube.com/watch?v={item.Id}";

            return new Musique(
                titre: item.Snippet.Title,
                duree: duration,
                album: item.Snippet.ChannelTitle ?? "", // fallback
                couverture: item.Snippet.Thumbnails.Medium?.Url ?? "",
                idYoutube: item.Id,
                lien: url
            );
        }).ToList();

        return musiques;
    }


    public async Task<bool> SaveToDatabaseAsync(Musique musique)
{
    string cs = $"Data Source=sql.reseau-labo.fr;Initial Catalog=watchflix;User ID=user_watchflix;Password=pwd_watchflix;TrustServerCertificate=True;";
    
    using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    
    string query = @"
        INSERT INTO Musique (titre_musique, duree_musique, album, couverture, youtube_id)
        VALUES (@titre, @duree, @album, @couverture, @youtube_id)";

    using var command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@titre", musique.Titre ?? "");
    command.Parameters.AddWithValue("@duree", musique.Duree);
    command.Parameters.AddWithValue("@album", musique.Album ?? "");
    command.Parameters.AddWithValue("@couverture", musique.Couverture ?? "");
    command.Parameters.AddWithValue("@youtube_id", musique.IdYoutube ?? "");
    command.Parameters.AddWithValue("@lien", musique.Lien ?? "");
    
    int rowsAffected = await command.ExecuteNonQueryAsync();
    return rowsAffected > 0;
}
}
