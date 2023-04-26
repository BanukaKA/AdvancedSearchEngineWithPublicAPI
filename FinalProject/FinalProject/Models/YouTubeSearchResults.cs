namespace FinalProject.Models
{
    public class YouTubeSearchResults
    {
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public Video[] Items { get; set; }
    }    

}
