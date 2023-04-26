using FinalProject.API;
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


        //Onclick Enter
        public async Task Enter(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                Search();
            }
        }


        //Search Functionality
        private async Task Search()
        {

            var response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={Jeeves.GoogleApiKey}&cx={Jeeves.GoogleSearchEngineID}&q={query}");
            results = response;

            response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={Jeeves.GoogleApiKey}&cx={Jeeves.GoogleSearchEngineID}&q={query}&searchType=image&fileType={imgType}");
            imageResults = response;

            response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={Jeeves.GoogleApiKey}&cx={Jeeves.GoogleSearchEngineID}&q={query}&fileType={fileType}");
            docResults = response;

            var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&q={query}&key={Jeeves.YoutubeApiKey}";
            var responseV = await Http.GetFromJsonAsync<YouTubeSearchResults>(url);
            videos = responseV;
            nextPageToken = videos.NextPageToken;
            IsDisabled = false;
        }


        //Image type changing
        private async Task ImgTypeChanged()
        {

            var response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={Jeeves.GoogleApiKey}&cx={Jeeves.GoogleSearchEngineID}&q={query}&searchType=image&fileType={imgType}");
            imageResults = response;

            await InvokeAsync(StateHasChanged);
        }

        //Doc Type Changing
        private async Task DocTypeChanged()
        {

            var response = await Http.GetFromJsonAsync<GoogleSearchResults>($"https://www.googleapis.com/customsearch/v1?key={Jeeves.GoogleApiKey}&cx={Jeeves.GoogleSearchEngineID}&q={query}&fileType={fileType}");
            docResults = response;

            await InvokeAsync(StateHasChanged);
        }

        //Paging
        private async Task NextPage()
        {
            videos = await Http.GetFromJsonAsync<YouTubeSearchResults>($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&pageToken={nextPageToken}&q={query}&key={Jeeves.YoutubeApiKey}");
            nextPageToken = videos.NextPageToken;
            prevPageToken = videos.PrevPageToken;
            IsDisabled = false;
        }
        private async Task PrevPage()
        {
            videos = await Http.GetFromJsonAsync<YouTubeSearchResults>($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=9&pageToken={prevPageToken}&q={query}&key={Jeeves.YoutubeApiKey}");
            nextPageToken = videos.NextPageToken;
            prevPageToken = videos.PrevPageToken;
        }

        //Speech Recognition

        [Parameter]
        public string Data { get; set; }

        //Page Initialized
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

        //Toggling Between Light and Dark Mode
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
