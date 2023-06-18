using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{

    public partial class SaveTTInterview
    {
        #region properties
        [Parameter]
        public EventCallback Close { get; set; }

        [Parameter]
        public CalendarInterview calendarSave { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        public bool statusLogin { get; set; } = false;
        public bool showuser { get; set; } = false;
        public List<UserInterview> UserInterviews { get; set; } = new List<UserInterview>();
        public int roominterview { get; set; }

        public bool isLoading { get; set; } = false;
        public List<CalendarInterview_UserInterview> interview_UserInterviews { get; set; } = new List<CalendarInterview_UserInterview>();


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
        /// - Get Data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            //get data candidate

            var listuserInterview = await httpClient.GetAsync("https://localhost:44365/api/UserInterview");

            UserInterviews = listuserInterview.Content.ReadFromJsonAsync<List<UserInterview>>().Result;

            var Calendar_UserInterview = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview_UserInterview");

            interview_UserInterviews = Calendar_UserInterview.Content.ReadFromJsonAsync<List<CalendarInterview_UserInterview>>().Result;
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Close Form lưu thông tin PV
        /// </summary>
        /// <returns></returns>
        public async Task CloseSaveTT()
        {
            isLoading = true;
            await Close.InvokeAsync();
            isLoading = false;
        }

        /// <summary>
        /// - Hiển thị form lưu thông tin phỏng vấn
        /// </summary>
        public void showUserInterview()
        {
            isLoading = true;
            showuser = !showuser;
            isLoading = false;
        }

  
        /// <summary>
        /// - Thức hiện thay đổi trạng thái selected của user phỏng vấn 
        /// </summary>
        /// <param name="id"></param>
        public void checkListUserInterview(int id)
        {
            isLoading = true;
            UserInterviews.FirstOrDefault(x => x.id == id).selected = !UserInterviews.FirstOrDefault(x => x.id == id).selected;
            isLoading = false;
        }

        /// <summary>
        /// - Lưu thông tin phỏng vấn
        /// - Gưi mail
        /// - Cập nhật trạng thái
        /// </summary>
        /// <returns></returns>
        public async Task saveTTCalendar()
        {
            isLoading = true;
            if (roominterview == 0)
            {
                await JS.InvokeVoidAsync("alert", "Mời Chọn Phòng Họp!");
            }
            else if (!UserInterviews.Any(x => x.selected == true)) 
            {
                await JS.InvokeVoidAsync("alert", "Mời Chọn Người Phỏng Vấn!");
            }
            else 
            {
                try
                {//Update Calendar
                    calendarSave.roominterview = roominterview;

                    var httpClient = new HttpClient();

                    await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarSave);

                    //add thông tin người phỏng vấn and buổi phỏng vấn
                    List<CalendarInterview_UserInterview> calendarInterview_UserInterviews = new List<CalendarInterview_UserInterview>();

                    UserInterviews.ForEach(async (x) =>
                    {
                        if (x.selected == true)
                        {
                            calendarInterview_UserInterviews.Add(new CalendarInterview_UserInterview(calendarSave.id, x.id));

                            //Send mail to user interview
                            MailRequest mailRequest = new MailRequest(x.email, "Mail Nhắc Lịch Phỏng Vấn", "Bạn được xếp lịch phỏng vấn ứng viên mới" + "<br/> Thời Gian :" + calendarSave.timeInterview + "Phòng Họp :" + calendarSave.roominterview + "<br/> Địa Điểm :" + calendarSave.addressInterview,x.name);

                            await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
                        }
                    });

                    if (calendarInterview_UserInterviews.Count != 0) 
                    {
                        if (interview_UserInterviews.Exists(x => calendarInterview_UserInterviews.FirstOrDefault().id_calendarinterview == x.id_calendarinterview))
                        {
                            await httpClient.DeleteAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + calendarInterview_UserInterviews.FirstOrDefault().id_calendarinterview);
                        }
                    }

                    //calendarInterview_UserInterviews.ForEach(async (x) => {  });

                    foreach (var x in calendarInterview_UserInterviews) 
                    {
                        var abc = await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview_UserInterview", x);
                    }
                    await JS.InvokeVoidAsync("ShowMessaggeSuccess");
                    await CloseSaveTT();
                }
                catch (Exception ex)
                {
                    await JS.InvokeVoidAsync("ShowMessaggeError");
                    throw;
                }


            }

            isLoading = false;
        }
        #endregion functions

    }
}
