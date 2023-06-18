using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using MySql.Data.MySqlClient;
using Google.Protobuf.WellKnownTypes;
using MyProject.Client.LoginManagements;

namespace MyProject.Client.Pages
{
    public partial class FilterCandidate
    {
        #region properties
        public List<Candidate> candidateFilters { get; set; } = new List<Candidate>();

        public List<Candidate> candidateFiltersSearch { get; set; } = new List<Candidate>();
        public List<Candidate> candidateFiltersFilter { get; set; } = new List<Candidate>();

        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();
        public bool viewCV { get; set; } = false;

        public string pathViewCV { get; set; } = string.Empty;

        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;

        public bool isLoading { get; set; } = false;
        public bool statusLogin { get; set; } = false;
        [Inject] IJSRuntime JS { get; set; }
        #endregion properties


        #region functions

        /// <summary>
        /// - Hiển thị khởi tạo
        /// </summary>
        protected override async void OnInitialized()
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
        /// - Get Data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var listCandidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/" + 1);
            candidateFiltersSearch = listCandidate.Content.ReadFromJsonAsync<List<Candidate>>().Result;
            candidateFilters = candidateFiltersSearch;

            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Show CV ứng viên
        /// </summary>
        /// <param name="id">id ứng viên muốn xem cv</param>
        /// <returns></returns>
        public async Task showSV(int id)
        {
            isLoading = true;
            pathViewCV = "";
            viewCV = true;
            var httpClient = new HttpClient();
            var Candidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate/" + id);

            Candidate candidate = await Candidate.Content.ReadFromJsonAsync<Candidate>();

            pathViewCV = "cvCandidate/" + candidate.Name + candidate.strID + "_CV.pdf";
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Close cpn view cv
        /// </summary>
        public void closeviewCV()
        {
            isLoading = true;
            viewCV = false;
            StateHasChanged();
            isLoading = false;
        }

        public async void browsingCandidate(int id)
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var Candidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate/" + id);

            Candidate candidate = await Candidate.Content.ReadFromJsonAsync<Candidate>();

            candidate.Status = 3;

            var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + id, candidate);

            getData();
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Loại ứng viên
        /// - Gửi mail thông báo xin lỗi loại ứng viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task removingCandidate(int id)
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var Candidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate/" + id);

            Candidate candidate = await Candidate.Content.ReadFromJsonAsync<Candidate>();

            candidate.Status = 2;
            candidate.Note = "Loại CV";
            string a = "1";

            var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + id, candidate);

            getData();
            StateHasChanged();
            isLoading = false;
        }

        #region search-filter

        /// <summary>
        /// - Tìm kiếm Candidate theo tên Candidate
        /// </summary>
        /// <param name="txt_search">Chuỗi tìm kếm nhập vào</param>
        public void search(string txt_search)
        {
            isLoading = true;
            string searchKeyword = txt_search.ToLower().Trim();
            //listOfObjectBySearch = listOfObject;
            //candidateFilters = candidateFiltersSearch;
            if (searchKeyword != "")
            {
                filterPositions(filterPosition.ToString());
                filterRoles(filterRole.ToString());
                candidateFilters = candidateFilters.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
            }
            else
            {
                filterPositions(filterPosition.ToString());
                filterRoles(filterRole.ToString());
                StateHasChanged();
            }
            isLoading = false;
        }

        /// <summary>
        /// - Lọc ứng viên theo Ngôn Ngữ
        /// </summary>
        /// <param name="positionFilter">id Ngôn Ngữ muốn lọc</param>
        public void filterPositions(string positionFilter)
        {
            isLoading = true;
            filterPosition = Int32.Parse(positionFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateFilters = candidateFiltersSearch;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }

        /// <summary>
        /// - Lọc ứng viên theo Vị Trí Ứng Tuyển
        /// </summary>
        /// <param name="roleFilter">id Vị Trí Ứng Tuyển muốn lọc </param>
        public void filterRoles(string roleFilter)
        {
            isLoading = true;
            filterRole = Int32.Parse(roleFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateFilters = candidateFiltersSearch;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateFilters = candidateFiltersSearch.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }
        #endregion search-filter
        #endregion functions
    }
}
