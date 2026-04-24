using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using watchflix.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
public class YoutubeServiceOfficial
{
    private readonly YouTubeService _youtube;
    public YoutubeServiceOfficial(string apiKey)
    {
        _youtube = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyAVze5iCrB-gvjas-GKwoa5TVJ2EsESfUo",
            ApplicationName = "WatchFlix"
        });
    }

    public async Task<List<Musique>> SearchMusicAsync(string query)
    {
        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "video";
        searchRequest.VideoCategoryId = "10"; // Musique
        searchRequest.MaxResults = 10;
        

        var searchResponse = await searchRequest.ExecuteAsync();

        var musiques = new List<Musique>();

        foreach (var item in searchResponse.Items)
        {
            var durationSpan = await GetVideoDurationAsync(item.Id.VideoId);

            string durationString = durationSpan.ToString(@"mm\:ss");
            musiques.Add(new Musique(
                titre: item.Snippet.Title,
                duree: durationSpan,
                album: item.Snippet.ChannelTitle ?? "",
                couverture: item.Snippet.Thumbnails.Medium?.Url ?? "",
                idYoutube: item.Id.VideoId,
                lien: ""
            ));
        }

        return musiques;
    }

    public async Task<TimeSpan> GetVideoDurationAsync(string videoId)
    {
        var videoRequest = _youtube.Videos.List("contentDetails");
        videoRequest.Id = videoId;

        var videoResponse = await videoRequest.ExecuteAsync();
        var duration = videoResponse.Items[0].ContentDetails.Duration;
        

        return System.Xml.XmlConvert.ToTimeSpan(duration);
    }
    public async Task<string> GetMusicLinkForMovieAsync(string movieTitle)
    {
        string query = $"{movieTitle} OST";
        
        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "video";
        searchRequest.VideoCategoryId = "10";
        searchRequest.MaxResults = 1;
        
        var searchResponse = await searchRequest.ExecuteAsync();
        
        if (searchResponse.Items != null && searchResponse.Items.Count > 0)
        {
            string videoId = searchResponse.Items[0].Id.VideoId;
            return $"https://www.youtube.com/watch?v={videoId}";
        }
        
        return null;
    }
    public async Task<Musique> GetMusicDetailsAsync(string movieTitle)
    {
        string query = $"{movieTitle} OST";
        
        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "video";
        searchRequest.VideoCategoryId = "10";
        searchRequest.MaxResults = 1;
        
        var searchResponse = await searchRequest.ExecuteAsync();
        
        if (searchResponse.Items != null && searchResponse.Items.Count > 0)
        {
            var item = searchResponse.Items[0];
            var duration = await GetVideoDurationAsync(item.Id.VideoId);
            
            return new Musique(
                titre:item.Snippet.Title,
                duree:duration,
                album:item.Snippet.ChannelTitle,
                couverture: item.Snippet.Thumbnails.Medium.Url,
                idYoutube : item.Id.VideoId,
                lien: ""
            );
        }  
        return null;
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
    
    int rowsAffected = await command.ExecuteNonQueryAsync();
    return rowsAffected > 0;
}
}
