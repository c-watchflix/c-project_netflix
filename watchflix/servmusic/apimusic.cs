using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using watchflix.Models;
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
                duree: durationSpan, // récupérable via GetVideoDetails
                album: "",
                couverture: item.Snippet.Thumbnails.Medium.Url
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
}
