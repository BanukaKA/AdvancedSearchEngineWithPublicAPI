using FinalProject.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using Toolbelt.Blazor.SpeechRecognition;

namespace FinalProject.Pages
{
    //Name : Banuka Kumara Ambegoda
    //Project Blazor PublicAPI
    public partial class Index
    {
        private string query;
        static string? InputValue;
        static bool InptAvail = false;
        private string nextPageToken;
        private string prevPageToken;
        private bool IsDisabled = true;
        private string fileType = "pdf";
        private string imgType;
        private bool isDarkMode = false;
        private GoogleSearchResults docResults;
        private GoogleSearchResults results;
        private GoogleSearchResults imageResults;
        private YouTubeSearchResults videos;

        private string transcription = "";

        public async Task Enter(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
  {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                Search();
            }
        }

        private async Task Search()
        {
            var apiKey = "AIzaSyCiIEeRSk2UpSerxGxStHL27X0YMBbi0LE ";
            var searchEngineId = "44b128b3a85a3465e";
            var response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={searchEngineId}&q={query}");
            results = response;

            response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={searchEngineId}&q={query}&searchType=image&fileType={imgType}");
            imageResults = response;

            response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={searchEngineId}&q={query}&fileType={fileType}");
            docResults = response;

            var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&q={query}&key=AIzaSyBHXprpxe_e-zU42QcDo_VXkxzOwBEW1wQ";
            var responseV = await Http.GetFromJsonAsync<YouTubeSearchResults>(url);
            videos = responseV;
            nextPageToken = videos.NextPageToken;
            IsDisabled = false;
        }

        private async Task NextPage()
        {
            videos = await Http.GetFromJsonAsync<YouTubeSearchResults>($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&pageToken={nextPageToken}&q={query}&key=AIzaSyBHXprpxe_e-zU42QcDo_VXkxzOwBEW1wQ");
            nextPageToken = videos.NextPageToken;
            prevPageToken = videos.PrevPageToken;
            IsDisabled = false;
        }
        private async Task PrevPage()
        {
            videos = await Http.GetFromJsonAsync<YouTubeSearchResults>($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&pageToken={prevPageToken}&q={query}&key=AIzaSyBHXprpxe_e-zU42QcDo_VXkxzOwBEW1wQ");
            nextPageToken = videos.NextPageToken;
            prevPageToken = videos.PrevPageToken;
        }

        //Speech Recognition

        [Parameter]
        public string Data { get; set; }


        protected async override void OnInitialized()
        {
            SpeechRecognition.Result += OnSpeechRecognized;
            query = Data;
            await Search();
            await InvokeAsync(StateHasChanged);
        }

        private async void OnSpeechRecognized(object sender, SpeechRecognitionEventArgs args)
        {
            if (args.Results.Length > 0 && args.Results[0].Items.Length > 0)
            {
                transcription = args.Results[0].Items[0].Transcript;
                query = transcription;
                await InvokeAsync(StateHasChanged);
                Search();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task StartSpeechRecognition()
        {
            await SpeechRecognition.StartAsync();
        }

        public void Dispose()
        {
            SpeechRecognition.Result -= OnSpeechRecognized;
        }

        private async Task ToggleTheme()
        {
            isDarkMode = !isDarkMode;
            if (isDarkMode)
            {
                await JSRuntime.InvokeVoidAsync("document.body.classList.add", "dark");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("document.body.classList.remove", "dark");
            }
        }
    }
}
