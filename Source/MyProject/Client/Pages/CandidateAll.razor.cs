using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class CandidateAll
    {

        #region properties

        public bool statusLogin { get; set; } = false;
        [Inject] IJSRuntime JS { get; set; }

        public List<Candidate> CandidatesAll { get; set; } = new List<Candidate>();
        public List<Candidate> CandidateEvaluatesTmp { get; set; } = new List<Candidate>();
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();

        public List<StatusCandidate> statusCandidates { get; set; } = new List<StatusCandidate>();

        public Candidate CandidateUpdate { get; set; } = new Candidate();

        public bool showCpnUpdate { get; set; } = false;


        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;
        public int? filterStt { get; set; } = 0;
        public bool isLoading { get; set; } = false;

        public int id_CandidateDel { set; get; } = 0;
        public bool isShowFormConfirmDelete { get; set; } = false;
        #endregion properties

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
            var listCandidates = await httpClient.GetAsync("https://localhost:44365/api/candidate");
            CandidatesAll = listCandidates.Content.ReadFromJsonAsync<List<Candidate>>().Result;

            CandidateEvaluatesTmp = CandidatesAll;

            statusCandidates = await httpClient.GetFromJsonAsync<List<StatusCandidate>>("https://localhost:44365/api/StatusCandidate");

            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            isLoading = false;
        }

        #region search-filter

        /// <summary>
        /// - TÌm kiếm qua tên
        /// </summary>
        /// <param name="txt_search"></param>
        public async void search(string txt_search)
        {
            isLoading = true;
            string searchKeyword = txt_search.ToLower().Trim();
            //listOfObjectBySearch = listOfObject;
            //candidateFilters = candidateFiltersSearch;
            if (searchKeyword != "")
            {
                CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
            }
            else
            {
                await getData();
                StateHasChanged();
            }
            isLoading = false;
        }

        public void ChangeFilter()
        {
            CandidateEvaluatesTmp = CandidatesAll.FindAll(x =>
                                                                (filterRole != 0 ? x.Role == filterRole : x.Role != filterRole)
                                                                && (filterPosition != 0 ? x.Position == filterPosition : x.Position != filterRole)
                                                                && (filterStt != 0 ? x.Status == filterStt : x.Status != filterRole));
            ////All
            //if (filterStt == 0 && filterRole == 0 && filterRole == 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll;
            //}

            //// 3 Điều Kiện
            //if (filterStt != 0 && filterRole != 0 && filterPosition != 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition && x.Status == filterStt);
            //}



            ////Filter Role - Status
            //if (filterRole != 0 && filterPosition == 0 && filterStt != 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Role == filterRole && x.Status == filterStt);
            //}

            ////Filter Position - Status
            //if (filterRole == 0 && filterPosition != 0 && filterStt != 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Position == filterPosition && x.Status == filterStt);
            //}

            ////Filter Role - Position
            //if (filterRole != 0 && filterPosition != 0 && filterStt == 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            //}



            ////Filter Role
            //if (filterRole != 0 && filterPosition == 0 && filterStt == 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Role == filterRole);
            //}

            ////Filter Position
            //if (filterRole == 0 && filterPosition != 0 && filterStt == 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Position == filterPosition);
            //}

            ////Filter Status
            //if (filterRole == 0 && filterPosition == 0 && filterStt != 0)
            //{
            //    CandidateEvaluatesTmp = CandidatesAll.FindAll(x => x.Status == filterStt);
            //}




            //CandidateEvaluatesTmp = CandidatesAll;

            //if (filterRole != 0)
            //{
            //    CandidateEvaluatesTmp = CandidateEvaluatesTmp.FindAll(x => x.Role == filterRole);
            //}
            //if (filterPosition != 0) 
            //{
            //    CandidateEvaluatesTmp = CandidateEvaluatesTmp.FindAll(x => x.Position == filterPosition);
            //}
            //if (filterStt != 0)
            //{
            //    CandidateEvaluatesTmp = CandidateEvaluatesTmp.FindAll(x => x.Status == filterStt);

            //}


        }


        public void ChangeDataFilter(ChangeEventArgs? dataP, ChangeEventArgs? dataR, ChangeEventArgs? dataS)
        {
            if (dataP != null)
            {
                filterPosition = Int32.Parse(dataP.Value.ToString());
                //fillers[0] = Int32.Parse(dataP.Value.ToString());
            }

            if (dataR != null)
            {
                filterRole = Int32.Parse(dataR.Value.ToString());
                //fillers[1] = Int32.Parse(dataP.Value.ToString());
            }

            if (dataS != null)
            {
                filterStt = Int32.Parse(dataS.Value.ToString());
                //fillers[2] = Int32.Parse(dataP.Value.ToString());
            }
            ChangeFilter();
        }
        #endregion search-filter


        #region functions

        public async void update(int id_candidate)
        {
            HttpClient httpClient = new HttpClient();
            CandidateUpdate = await httpClient.GetFromJsonAsync<Candidate>("https://localhost:44365/api/Candidate/" + id_candidate);
            showCpnUpdate = true;
            StateHasChanged();
        }
        public void showFormCFDelete(int id_candidate)
        {
            id_CandidateDel = id_candidate;
            isShowFormConfirmDelete = true;
            StateHasChanged();
        }
        public async void delete(int id_candidate)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                await httpClient.DeleteAsync("https://localhost:44365/api/Candidate/" + id_candidate);
                await getData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            StateHasChanged();
        }
        public async Task closeUpdate()
        {
            showCpnUpdate = false;
            await getData();
            StateHasChanged();
        }

        public async void closeFormCFDel()
        {
            isShowFormConfirmDelete = false;
            await getData();
            StateHasChanged();
        }
        #endregion functions
    }
}
