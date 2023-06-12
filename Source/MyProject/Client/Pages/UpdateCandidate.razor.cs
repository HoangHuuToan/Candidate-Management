using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class UpdateCandidate
    {
        [Parameter]
        public Candidate candidate { get; set; } = new Candidate();
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();

        public List<ValuesOfComb> valuesOfRole { get; set; } = new List<ValuesOfComb>();

        public List<ValuesOfComb> valuesOfPosition { get; set; } = new List<ValuesOfComb>();

        public List<StatusCandidate> statusCandidates { get; set; } = new List<StatusCandidate>();

        [Parameter]
        public EventCallback Close { get; set; }
        public bool statusLogin { get; set; } = false;
        public bool isLoading { get; set; } = false;

        [Inject] IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {

            isLoading = true;
            LoginManagement loginManagement = new LoginManagement(JS);
            statusLogin = await loginManagement.checkLogin();
            if (statusLogin)
            {
                await getData();
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("Logout");
            }
            isLoading = false;
            StateHasChanged();

        }


        /// <summary>
        /// - get Data hiển thị 
        /// </summary>0
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            HttpClient httpClient = new HttpClient();
            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            valuesOfPosition = valuesOfCombs.FindAll((x) => x.valueOfComb == 1);
            valuesOfRole = valuesOfCombs.FindAll((x) => x.valueOfComb == 2);

            statusCandidates = await httpClient.GetFromJsonAsync<List<StatusCandidate>>("https://localhost:44365/api/StatusCandidate");

            isLoading = false;
        }
        public void CloseThis()
        {
            Close.InvokeAsync();
            StateHasChanged();
        }
        public async Task update()
        {
            isLoading = true;
            StateHasChanged();
            try
            {
                HttpClient httpClient = new HttpClient();
                await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidate.id, candidate);
                await JS.InvokeVoidAsync("alert", "Thành Công");
                CloseThis();
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Không Thành Công");
                Console.WriteLine(ex);
            }
            isLoading = false;
            StateHasChanged();
        }
    }
}
