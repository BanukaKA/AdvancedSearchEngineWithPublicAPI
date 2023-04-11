namespace FinalProject.Models
{
    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
    }
}
