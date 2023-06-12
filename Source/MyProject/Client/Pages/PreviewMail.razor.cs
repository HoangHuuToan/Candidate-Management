using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http.Json;
namespace MyProject.Client.Pages
{
    public partial class PreviewMail
    {
        #region properties
        public string addressMailSSV { get; set; } = "hoanghuutoanit@gmail.com";
        [Parameter]
        public Candidate candidate { get; set; }

        [Parameter]
        public EventCallback Close { get; set; }

        [Parameter]
        public EventCallback changeData { get; set; }

        [Parameter]
        public TemplateMail templatemail { get; set; } = new TemplateMail();

        public List<CalendarInterview> CalendarInterviewAll { get; set; } = new List<CalendarInterview>();
        [Inject] IJSRuntime JS { get; set; }
        public bool statusLogin { get; set; } = false;
        public int typeMail { get; set; }

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
        /// Close cpn prv mail
        /// </summary>
        public void ClosePrv()
        {
            isLoading = true;
            Close.InvokeAsync();
            isLoading = false;
        }

        /// <summary>
        /// - Get data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            var httpClient = new HttpClient();
            var carlendarPVV1 = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview/stt/2");

            var carlendarAll = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview");
            CalendarInterviewAll = carlendarAll.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;
            isLoading = false;
        }

        /// <summary>
        /// - Gửi mail cho ứng viên
        /// </summary>
        /// <returns></returns>
        public async Task sendMail()
        {
            isLoading = true;
            var httpClient = new HttpClient();
            try
            {
                CalendarInterview calendarInterview = new CalendarInterview();

                switch (candidate.Status)
                {
                    case 5:
                        calendarInterview.nameInterview = "PV V1";
                        calendarInterview.idCandidate = candidate.id;
                        calendarInterview.timeInterview = candidate.timeInterview;
                        calendarInterview.addressInterview = candidate.addressInterview;
                        //candidate.Contacting = 0;
                        candidate.Status = 6;
                        break;
                    case 7:
                        calendarInterview.nameInterview = "PV V2";
                        calendarInterview.idCandidate = candidate.id;
                        calendarInterview.timeInterview = candidate.timeInterview;
                        calendarInterview.addressInterview = candidate.addressInterview;
                        //candidate.Contacting = 0;
                        candidate.Status = 8;
                        break;

                }


                MailRequest mailRequest = new MailRequest(candidate.Email, templatemail.tittleMail, templatemail.contentMail + "<br/> Thời Gian :" + candidate.timeInterview + "<br/> Địa Điểm :" + candidate.addressInterview, candidate.Name);

                var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
                var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidate.id, candidate);

                bool add = true;
                foreach (var calendar in CalendarInterviewAll)
                {
                    if (calendarInterview.idCandidate == calendar.idCandidate && calendarInterview.nameInterview == calendar.nameInterview)
                    {
                        await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarInterview);
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview/add", calendarInterview);
                }

                await getData();
                await JS.InvokeVoidAsync("alert", "Thành công!");
                ClosePrv();
                StateHasChanged();

            }
            catch
            {
                await JS.InvokeVoidAsync("alert", "Không Thành công!");
            }

            isLoading = false;

        }
        #endregion functions
    }
}
