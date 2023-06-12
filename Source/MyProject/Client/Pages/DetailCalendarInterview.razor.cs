
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
    public partial class DetailCalendarInterview
    {

        #region properties

        public bool statusLogin { get; set; } = false;
        [Parameter]
        public CalendarInterview calendarInterview { get; set; } = new CalendarInterview();

        public List<CalendarInterview_UserInterview> interview_UserInterviews { get; set; } = new List<CalendarInterview_UserInterview>();
        [Parameter]
        public List<UserInterview> userInterviews { get; set; } = new List<UserInterview>();
        [Inject] IJSRuntime JS { get; set; }

        [Parameter]
        public EventCallback Close { get; set; }

        public bool showCfDlt { get; set; } = false;

        public bool isLoading { get; set; } = false;
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
            calendarInterview.idUserInterviews.ForEach((x) =>
            {
                userInterviews.ForEach((y) =>
                {
                    if (x == y.id)
                    {
                        y.selected = true;
                    }

                });

            });
            var abc = userInterviews;

            var httpClient = new HttpClient();
            var Calendar_UserInterview = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview_UserInterview");

            interview_UserInterviews = Calendar_UserInterview.Content.ReadFromJsonAsync<List<CalendarInterview_UserInterview>>().Result;


            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Close cpn xem chi tiết lịch PV
        /// </summary>
        /// <returns></returns>
        public async Task Closethis()
        {
            isLoading = true;
            await Close.InvokeAsync();
            isLoading = false;
        }

        /// <summary>
        /// -Update lịch PV hiện tại
        /// </summary>
        /// <returns></returns>
        public async Task update()
        {
            isLoading = true;
            try
            {
                //update lịch
                var httpClient = new HttpClient();
                await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarInterview);

                //add thông tin người phỏng vấn and buổi phỏng vấn
                List<CalendarInterview_UserInterview> calendarInterview_UserInterviews = new List<CalendarInterview_UserInterview>();

                userInterviews.ForEach(async (x) =>
                {
                    if (x.selected == true)
                    {
                        calendarInterview_UserInterviews.Add(new CalendarInterview_UserInterview(calendarInterview.id, x.id));

                        //Send mail to user interview
                        MailRequest mailRequest = new MailRequest(x.email, "Mail Nhắc Lịch Phỏng Vấn", "Bạn được xếp lịch phỏng vấn ứng viên mới" + "<br/> Thời Gian :" + calendarInterview.timeInterview + "Phòng Họp :" + calendarInterview.roominterview + "<br/> Địa Điểm :" + calendarInterview.addressInterview, x.name);

                        await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
                    }
                });

                if (interview_UserInterviews.Exists(x => calendarInterview_UserInterviews.FirstOrDefault().id_calendarinterview == x.id_calendarinterview))
                {
                    await httpClient.DeleteAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + calendarInterview_UserInterviews.FirstOrDefault().id_calendarinterview);
                }

                calendarInterview_UserInterviews.ForEach(async (x) => { await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview_UserInterview", x); });
                await JS.InvokeVoidAsync("alert", "Update thành công !");
                await Closethis();
            }
            catch
            {
                await JS.InvokeVoidAsync("alert", "Update lỗi !!!");
            }

            StateHasChanged();
            isLoading = false;

        }

        /// <summary>
        /// - Cập nhật trạng thái selected của nhân viên phỏng vấn
        /// </summary>
        /// <param name="id">id của nhân viên phỏng vấn</param>
        public void checkListUserInterview(int id)
        {
            isLoading = true;
            userInterviews.FirstOrDefault(x => x.id == id).selected = !userInterviews.FirstOrDefault(x => x.id == id).selected;
            StateHasChanged();
            isLoading = false;

        }

        /// <summary>
        /// - Delete xóa lịch phỏng vấn hiện tại
        /// </summary>
        /// <param name="id">id của lịch phỏng vấn hiện tại</param>
        /// <returns></returns>
        public async Task delete(int id)
        {
            isLoading = true;
            var httpClient = new HttpClient();
            //delete Lịch - Phòng
            var dlCalendarInterview = await httpClient.DeleteAsync("https://localhost:44365/api/CalendarInterview/" + id);

            //Xóa lịch - Ng phỏng vấn
            var dlCalendarUserInterview = await httpClient.DeleteAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + id);
            CloseConfirmDlt();
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Hiển thị cpn confirm xóa ?
        /// </summary>
        public void showConfirmDlt()
        {
            isLoading = true;
            showCfDlt = true;
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Đóng cpn confirm xóa
        /// </summary>
        public void CloseConfirmDlt()
        {
            isLoading = true;
            showCfDlt = false;
            StateHasChanged();
            isLoading = false;
        }
        #endregion functions
    }
}
