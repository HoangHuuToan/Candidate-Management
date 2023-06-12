using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;
namespace MyProject.Client.Pages
{
    public partial class ScreenCalendarInterview
    {
        #region properties
        public List<CalendarInterview_UserInterview> calendarInterview_UserInterviews { get; set; } = new List<CalendarInterview_UserInterview>();


        public List<CalendarInterview> calendarInterviewshasroomAll { get; set; } = new List<CalendarInterview> { };

        public List<CalendarInterview> calendarInterviewshasroom { get; set; } = new List<CalendarInterview> { };
        public List<UserInterview> userInterviews { get; set; } = new List<UserInterview> { };
        public bool isShowDetail { get; set; } = false;
        public CalendarInterview calendarInterviewViewDetail { get; set; } = new CalendarInterview();
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
        /// -Get data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            // Lịch Phỏng vấn - Người Phỏng vấn
            var calendar_users = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview_UserInterview");
            calendarInterview_UserInterviews = calendar_users.Content.ReadFromJsonAsync<List<CalendarInterview_UserInterview>>().Result;

            // Lịch - Thời gian phỏng vấn có phòng
            var calendars = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview/hasroom");
            calendarInterviewshasroomAll = calendars.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;
            calendarInterviewshasroom = calendarInterviewshasroomAll;


            //Data người phỏng vấn
            var datauserInterview = await httpClient.GetAsync("https://localhost:44365/api/UserInterview");
            userInterviews = datauserInterview.Content.ReadFromJsonAsync<List<UserInterview>>().Result;

            

            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");
            foreach (var carlendar  in calendarInterviewshasroom) 
            {
                foreach (var userInterview in calendarInterview_UserInterviews) 
                {
                    if (carlendar.id == userInterview.id_calendarinterview) 
                    {
                        carlendar.idUserInterviews.Add(userInterview.id_userinterview);
                        carlendar.nameUserInterviews.Add(userInterviews.FirstOrDefault(x => x.id == userInterview.id_userinterview).name);
                    }
                
                }
            }

            StateHasChanged();
            isLoading = false;
        }


        #region search-filter
        /// <summary>
        /// - Tìm kiếm danh sách theo tiên
        /// </summary>
        /// <param name="txt_search"></param>
        /// <returns></returns>
        public async Task search(string txt_search)
        {
            isLoading = true;
            string searchKeyword = txt_search.ToLower().Trim();
            //listOfObjectBySearch = listOfObject;
            //candidateFilters = candidateFiltersSearch;
            if (searchKeyword != "")
            {
                calendarInterviewshasroom = calendarInterviewshasroomAll.FindAll(x => x.nameCandidate.ToLower().Trim().Contains(searchKeyword));
            }
            else
            {
                await getData();
                StateHasChanged();
            }
            isLoading = false;
        }
        #endregion search-filter

        /// <summary>
        /// - xem Detail , chi tiết Lịch PV
        /// </summary>
        /// <param name="id"></param>
        public void viewDetail(int id) 
        {
            isLoading = true;
            isShowDetail =  true;
            calendarInterviewViewDetail = calendarInterviewshasroom.FirstOrDefault(x => x.id == id);
            calendarInterviewViewDetail.roominterview = calendarInterviewshasroom.FirstOrDefault((x)=> x.id == calendarInterviewViewDetail.id).roominterview;
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Đóng cpn View detail calendar
        /// </summary>
        /// <returns></returns>
        public async Task CloseDetail() 
        {
            isLoading = true;
            isShowDetail = false;
            await getData();
            StateHasChanged();
            isLoading = false;
        }

        #endregion functions
    }
}
