namespace FinalProject.Models
{
    public class YouTubeSearchResults
    {
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public Video[] Items { get; set; }
    }
    public class Video
    {
        public Id id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Id
    {
        public string videoId { get; set; }
    }

    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
    }

    public class Thumbnails
    {
        public Thumbnail medium { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
    }

}
