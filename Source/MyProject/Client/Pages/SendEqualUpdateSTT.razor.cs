using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class SendEqualUpdateSTT
    {
        #region properties
        public List<Candidate> CandidateEvaluates { get; set; } = new List<Candidate>();
        public List<Candidate> CandidateEvaluatesTmp { get; set; } = new List<Candidate>();

        public Candidate CandidateSendOffer { get; set; } = new Candidate();

        public List<Candidate> EvaluateCandidateViewDetail { get; set; } = new List<Candidate>();
        public bool showFormSendOffer { get; set; } = false;

        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();
        public List<StatusCandidate> statusCandidates { get; set; } = new List<StatusCandidate>();

        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;
        public int? filterStt { get; set; } = 0;
        public bool isLoading { get; set; } = false;

        public bool statusLogin { get; set; } = false;
        [Inject] IJSRuntime JS { get; set; }

        #endregion properties

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
                CandidateEvaluatesTmp = CandidateEvaluates.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
            }
            else
            {
                await getData();
                StateHasChanged();
            }
            isLoading = false;
            StateHasChanged();

        }

        public void ChangeFilter()
        {
            CandidateEvaluatesTmp = CandidateEvaluates.FindAll(x =>
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
            StateHasChanged();

        }
        #endregion search-filter


        #region functions

        /// <summary>
        /// Hiền thị khởi tạo
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
        /// - Get Data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            var listCandidates = await httpClient.GetAsync("https://localhost:44365/api/candidate/sendoffer");
            CandidateEvaluates = listCandidates.Content.ReadFromJsonAsync<List<Candidate>>().Result;

            CandidateEvaluatesTmp = CandidateEvaluates;

            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");
            statusCandidates = await httpClient.GetFromJsonAsync<List<StatusCandidate>>("https://localhost:44365/api/StatusCandidate");
            isLoading = false;
            StateHasChanged();
        }

        /// <summary>
        /// - Gửi mail offer cho ứng viên
        /// </summary>
        /// <param name="id_candidate">id của Candidate nhận mail</param>
        /// <returns></returns>
        public async Task SendOffer(int id_candidate)
        {
            isLoading = true;
            CandidateSendOffer = CandidateEvaluates.FirstOrDefault((x)=> x.id == id_candidate);
            showFormSendOffer = true;
            isLoading = false;
            StateHasChanged();

        }


        /// <summary>
        /// - Đóng form send Offer
        /// </summary>
        public async void CloseFormSendOffer() 
        {
            isLoading = true;
            await getData();
            showFormSendOffer = false;
            isLoading = false;
            StateHasChanged();

        }
        #endregion functions

    }
}
