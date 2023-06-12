using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyProject.Shared.Entities;
using MySqlX.XDevAPI;
using System.Net.Http.Json;
using MyProject.Client.LoginManagements;

namespace MyProject.Client.Pages
{
    public partial class AddTTInterview
    {
        #region properties

        [Inject] IJSRuntime JS { get; set; }
        public List<Candidate> candidateaddTTinterview { get; set; } = new List<Candidate>();

        public bool checkall { get; set; } = false;

        public List<CalendarInterview> CalendarInterviewPVV1 { get; set; } = new List<CalendarInterview>();
        public List<CalendarInterview> CalendarInterviewPVV2 { get; set; } = new List<CalendarInterview>();
        public List<CalendarInterview> CalendarInterviewAll { get; set; } = new List<CalendarInterview>();
        public bool prvMail { get; set; } = false;

        public List<Candidate> candidateAddAll { get; set; } = new List<Candidate>();
        public Candidate candidateprvMail { get; set; } = new Candidate();
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();

        public CalendarInterview calendarSave = new CalendarInterview();

        public bool statusLogin { get; set; } = false;
        //Mail

        public TemplateMail mailCurrently { get; set; } = new TemplateMail();

        public List<TemplateMail> templatemails { get; set; } = new List<TemplateMail> { };
        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;
        public int typeMail { get; set; }
        public bool ShowSaveCpn { get; set; } = false;

        public bool isLoading { get; set; } = false;
        #endregion properties

        #region Functions

        /// <summary>
        /// Hiển thị khởi tạo
        /// </summary>
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
        /// - Get Data hiển thị khởi tạo lên component 
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            

            isLoading = true;
            var httpClient = new HttpClient();

            //get data candidate

            var listCandidateAdd = await httpClient.GetAsync("https://localhost:44365/api/Candidate/AddTTInterview");

            //var listCandidateSentv2 = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/8");

            //Add data Candidate
            candidateAddAll = listCandidateAdd.Content.ReadFromJsonAsync<List<Candidate>>().Result;

            candidateaddTTinterview = candidateAddAll;

            //listCandidateSentv2.Content.ReadFromJsonAsync<List<Candidate>>().Result.ForEach(x => candidateaddTTinterview.Add(x));

            //listCandidateSentv1.Content.ReadFromJsonAsync<List<Candidate>>().Result.ForEach(x => candidateSendMails.Add(x));

            //listCandidateSentv2.Content.ReadFromJsonAsync<List<Candidate>>().Result.ForEach(x => candidateSendMails.Add(x));


            //Calendar Interview

            var carlendarPVV1 = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview/stt/2");

            CalendarInterviewPVV1 = carlendarPVV1.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;


            var carlendarPVV2 = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview/stt/3");

            CalendarInterviewPVV2 = carlendarPVV2.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;


            var carlendarAll = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview");

            CalendarInterviewAll = carlendarAll.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;

            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            //Mail

            var templateMail = await httpClient.GetAsync("https://localhost:44365/api/TemplateMail");

            templatemails = templateMail.Content.ReadFromJsonAsync<List<TemplateMail>>().Result;
            //
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">Data trên đối tượng mỗi khi có sự thay đổi</param>
        /// <param name="id">id của Canidate hiện tại cần thay đổi</param>
        public void rf(ChangeEventArgs e, int id)
        {
            isLoading = true;
            var Candidatempt = candidateaddTTinterview.FirstOrDefault(c => c.id == id);

            if (Candidatempt != null)
            {
                Candidatempt.Contacting = int.Parse(e.Value.ToString());
            }

            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_calendar">id của lich cần lưu thông tin</param>
        /// <param name="id_candidate">id của Candidate cần lưu lịch</param>
        public void showSaveTTCpn(int id_calendar, int id_candidate)
        {
            isLoading = true;
            var calendarSavempt = CalendarInterviewAll.FirstOrDefault(x => x.id == id_calendar);
            if (calendarSavempt != null)
            {
                calendarSave = calendarSavempt;


            }


            //when change value
            var a = candidateaddTTinterview.FirstOrDefault(x => x.id == id_candidate);
            if (a != null)
            {
                calendarSave.timeInterview = a.timeInterview;
            }


            var b = candidateaddTTinterview.FirstOrDefault(x => x.id == id_candidate);
            if (b != null)
            {
                calendarSave.addressInterview = b.addressInterview;
            }

            ShowSaveCpn = true;
            isLoading = false;
        }


        /// <summary>
        /// - Đón cpn lưu thông tin PV
        /// </summary>
        public void CloseSaveTTCpn()
        {
            isLoading = true;
            ShowSaveCpn = false;
            isLoading = false;
        }

        #region Search-Filter
        /// <summary>
        /// Tìm kiếm trong danh sách qua tên
        /// </summary>
        /// <param name="txt_search">Chuỗi người dùng nhập vào</param>
        /// <returns></returns>
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
                candidateaddTTinterview = candidateaddTTinterview.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
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
        /// - Lọc danh sách ứng viên theo vị trí
        /// </summary>
        /// <param name="positionFilter">id vị trí cần lọc</param>
        public void filterPositions(string positionFilter)
        {
            isLoading = true;
            filterPosition = Int32.Parse(positionFilter);
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateaddTTinterview = candidateAddAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }

        /// <summary>
        /// - Lọc danh sách ứng viên theo chức danh
        /// </summary>
        /// <param name="roleFilter">id chức danh cần lọc</param>
        public void filterRoles(string roleFilter)
        {
            isLoading = true;
            filterRole = Int32.Parse(roleFilter);
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateaddTTinterview = candidateAddAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateaddTTinterview = candidateAddAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }
        #endregion Search-Filter
        #endregion Functions


    }
}
