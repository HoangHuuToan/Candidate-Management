using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using Syncfusion.Blazor.Inputs;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class ImportEvaluateCandidate
    {

        #region properties
        public List<Candidate> CandidateEvaluatesAll { get; set; } = new List<Candidate>();
        public List<Candidate> CandidateEvaluates { get; set; } = new List<Candidate>();
        public Candidate candidateEvaluate { get; set; }
        public bool showformevaluate { get; set; } = false;

        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();
        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;

        public bool isLoading { get; set; } = false;

        public bool statusLogin { get; set; } = false;
        [Inject] IJSRuntime JS { get; set; }

        #endregion properties

        /// <summary>
        /// -Hiển thị khởi tạo 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            LoginManagement loginManagement = new LoginManagement(JS);
            statusLogin = await loginManagement.checkLogin();
            if (statusLogin)
            {
                await getData();
            }
            else
            {
                await JS.InvokeVoidAsync("Logout");
            }
            isLoading = false;
            StateHasChanged();
        }


        /// <summary>
        /// - Get data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var listCandidates = await httpClient.GetAsync("https://localhost:44365/api/candidate/screencalendarinterview");

            CandidateEvaluatesAll = listCandidates.Content.ReadFromJsonAsync<List<Candidate>>().Result;
            CandidateEvaluates = CandidateEvaluatesAll;


            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            StateHasChanged();
            isLoading = false;
        }

        #region search-filter

        /// <summary>
        /// - TÌm kiếm qua tên
        /// </summary>
        /// <param name="txt_search"></param>
        public void search(string txt_search)
        {
            isLoading = true;
            string searchKeyword = txt_search.ToLower().Trim();
            //listOfObjectBySearch = listOfObject;
            //candidateFilters = candidateFiltersSearch;
            if (searchKeyword != "")
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
            }
            else
            {
                getData();
                StateHasChanged();
            }
            isLoading = false;
        }

        /// <summary>
        /// -Lọc theo Ngôn Ngữ
        /// </summary>
        /// <param name="e">Data nhận mỗi khi có change event</param>
        public void filterPositions(ChangeEventArgs e)
        {
            isLoading = true;
            filterPosition = Int32.Parse(e.Value.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }


        /// <summary>
        /// -Lọc dánh sách qua Vị Trí Ứng Tuyển
        /// </summary>
        /// <param name="e">Data nhận vào mỗi khi có change event</param>
        public void filterRoles(ChangeEventArgs e)
        {
            isLoading = true;
            filterRole = Int32.Parse(e.Value.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                CandidateEvaluates = CandidateEvaluatesAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }
        #endregion search-filter

        /// <summary>
        /// -Hiển thị form đánh giá ứng viên
        /// </summary>
        /// <param name="id"></param>
        public void showFormEvaluate(int id)
        {
            isLoading = true;
            candidateEvaluate = CandidateEvaluates.FirstOrDefault((x) => x.id == id);
            showformevaluate = true;
            StateHasChanged();
            isLoading = false;
        }
        /// <summary>
        /// - Đóng form đánh giá ứng viên
        /// </summary>
        /// <returns></returns>
        public async Task closeFormEvaluate()
        {
            isLoading = true;
            await getData();
            showformevaluate = false;
            StateHasChanged();
            isLoading = false;
        }

    }
}
