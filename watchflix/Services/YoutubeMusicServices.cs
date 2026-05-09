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
        // 1. Construire la requête
        string query = $"{movieTitle} soundtrack complete";

        // 2. Rechercher une playlist
        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "playlist";
        searchRequest.MaxResults = 1;

        var searchResponse = await searchRequest.ExecuteAsync();

        if (searchResponse.Items == null || searchResponse.Items.Count == 0)
            return new List<Musique>();

        // 3. Récupérer l'ID de la playlist
        var playlistId = searchResponse.Items.First().Id.PlaylistId;

        if (string.IsNullOrEmpty(playlistId))
            return new List<Musique>();

        // 4. Récupérer les vidéos de la playlist
        var playlistItems = new List<Google.Apis.YouTube.v3.Data.PlaylistItem>();
        string nextPageToken = null;

        do
        {
            var playlistRequest = _youtube.PlaylistItems.List("snippet,contentDetails");
            playlistRequest.PlaylistId = playlistId;
            playlistRequest.MaxResults = 50;
            playlistRequest.PageToken = nextPageToken;

            var playlistResponse = await playlistRequest.ExecuteAsync();

            if (playlistResponse.Items != null)
                playlistItems.AddRange(playlistResponse.Items);

            nextPageToken = playlistResponse.NextPageToken;

        } while (nextPageToken != null);

        if (playlistItems.Count == 0)
            return new List<Musique>();

        // 5. Récupérer les IDs des vidéos
        var videoIds = playlistItems
            .Select(i => i.ContentDetails.VideoId)
            .Where(id => !string.IsNullOrEmpty(id))
            .ToList();

        // 6. Récupérer les détails des vidéos (durée, titre…)
        var videoRequest = _youtube.Videos.List("contentDetails,snippet");
        videoRequest.Id = string.Join(",", videoIds);

        var videoResponse = await videoRequest.ExecuteAsync();

        // 7. Transformer en objets Musique
        var musiques = videoResponse.Items.Select(item =>
        {
            var duration = System.Xml.XmlConvert.ToTimeSpan(item.ContentDetails.Duration);

            string url = $"https://www.youtube.com/watch?v={item.Id}";

            return new Musique(
                titre: item.Snippet.Title,
                duree: duration,
                album: item.Snippet.ChannelTitle ?? "",
                couverture: item.Snippet.Thumbnails.Medium?.Url ?? "",
                idYoutube: item.Id,
                lien: url,
                selectionne: true
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
            INSERT INTO Musique (titre_musique, duree_musique, album, couverture, youtube_id, lien)
            VALUES (@titre, @duree, @album, @couverture, @youtube_id, @lien)";

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
