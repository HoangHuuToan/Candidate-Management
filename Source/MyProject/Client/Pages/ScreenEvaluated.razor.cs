using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class ScreenEvaluated
    {
        #region properties
        public List<Candidate> CandidateEvaluatesAll { get; set; } = new List<Candidate>();
        public List<Candidate> CandidateEvaluates { get; set; } = new List<Candidate>();

        public Candidate GradeTestCandidate { get; set; } = new Candidate();

        public List<Candidate> EvaluateCandidateViewDetail { get; set; } = new List<Candidate>();
        public bool showformevaluate { get; set; } = false;
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();

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
        /// <returns></returns>
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
        /// - Get data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var listCandidates = await httpClient.GetAsync("https://localhost:44365/api/candidate/screenevaluated");

            CandidateEvaluatesAll = listCandidates.Content.ReadFromJsonAsync<List<Candidate>>().Result;
            CandidateEvaluates = CandidateEvaluatesAll;
            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            StateHasChanged();
            isLoading = false;
        }


        #region search-filter

        /// <summary>
        /// - Tìm kiếm danh sách theo tên
        /// </summary>
        /// <param name="txt_search">Chuỗi tên nhập vào tìm kiếm</param>
        /// <returns></returns>
        public async Task search(string txt_search)
        {
            isLoading = true;
            string searchKeyword = txt_search.ToLower().Trim();
            //listOfObjectBySearch = listOfObject;
            //candidateFilters = candidateFiltersSearch;
            if (searchKeyword != "")
            {
                filterPositions(filterPosition.ToString());
                filterRoles(filterRole.ToString());
                CandidateEvaluates = CandidateEvaluates.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
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
        /// - Lọc theo vị trí
        /// </summary>
        /// <param name="positionFilter">id vị trí cần lọc</param>
        public void filterPositions(string positionFilter)
        {
            isLoading = true;
            filterPosition = Int32.Parse(positionFilter.ToString());
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
        /// - Lọc theo chức danh 
        /// </summary>
        /// <param name="roleFilter">id chức danh cân lọc</param>
        public void filterRoles(string roleFilter)
        {
            isLoading = true;
            filterRole = Int32.Parse(roleFilter.ToString());
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
        /// -Xem Đánh giá
        /// </summary>
        /// <param name="id_candidate">id của lịch pv cần xem</param>
        /// <returns></returns>
        public async Task viewEvaluate(int id_candidate)
        {
            isLoading = true;
            var httpClient = new HttpClient();

            //Grade Test
            var GradeTestOfCandidate = await httpClient.GetAsync("https://localhost:44365/api/candidate/gradetestCandidate/"+id_candidate);

            GradeTestCandidate = GradeTestOfCandidate.Content.ReadFromJsonAsync<Candidate>().Result;
            //Evaluate Candidate Detail

            var evaluateCandidate = await httpClient.GetAsync("https://localhost:44365/api/candidate/evaluatedofCandidate/" + id_candidate);

            EvaluateCandidateViewDetail = evaluateCandidate.Content.ReadFromJsonAsync<List<Candidate>>().Result;

            // show Detail Evaluate 
            showformevaluate = true;
            isLoading = false;

        } 


        /// <summary>
        /// - Đóng cpn xem đánh  giá 
        /// </summary>
        public void CloseviewEvaluate() 
        {
            isLoading = true;
            showformevaluate = false;
            isLoading = false;
        }
        #endregion functions


    }
}
