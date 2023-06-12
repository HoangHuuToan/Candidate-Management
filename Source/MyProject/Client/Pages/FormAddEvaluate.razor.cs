using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class FormAddEvaluate
    {

        #region properties
        [Parameter]
        public EventCallback closeFormthis { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Parameter]
        public Candidate CandidateEvaluate { get; set; }

        public List<CalendarInterview_UserInterview> calendarInterview_UserInterviews { get; set; } = new List<CalendarInterview_UserInterview> { };

        public List<CalendarInterview_UserInterview> calendarInterview_UserInterviewsUnevaluate { get; set; } = new List<CalendarInterview_UserInterview> { };
        public List<TemplateMail> templatemails { get; set; } = new List<TemplateMail> { };
        public int equal { get; set; } = 1;
        public int id_user_evaluate { get; set; }
        public string note { get; set; } = string.Empty;
        public bool isLoading { get; set; } = false;
        public bool statusLogin { get; set; } = false;
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

            var calendar_userinterviews = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + CandidateEvaluate.id_calendar);

            calendarInterview_UserInterviews = calendar_userinterviews.Content.ReadFromJsonAsync<List<CalendarInterview_UserInterview>>().Result;


            calendarInterview_UserInterviewsUnevaluate = calendarInterview_UserInterviews.Where((x) => x.evaluate == 1).ToList();

            //Mail

            var templateMail = await httpClient.GetAsync("https://localhost:44365/api/TemplateMail");

            templatemails = templateMail.Content.ReadFromJsonAsync<List<TemplateMail>>().Result;

            if (calendarInterview_UserInterviewsUnevaluate.FirstOrDefault() != null)
            {
                id_user_evaluate = calendarInterview_UserInterviewsUnevaluate.FirstOrDefault().id_userinterview;
            }

            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Đánh giá , cập nhật , gửi mail cho ứng viên
        /// </summary>
        /// <returns></returns>
        public async Task Evaluate()
        {
            isLoading = true;
            TemplateMail templatemail = new TemplateMail();
            try
            {
                var httpClient = new HttpClient();


                CalendarInterview_UserInterview calendarInterview_UserInterviewUpdate = calendarInterview_UserInterviews.FirstOrDefault((x) => x.id_userinterview == id_user_evaluate);

                calendarInterview_UserInterviewUpdate.evaluate = equal;
                calendarInterview_UserInterviewUpdate.note_evaluate = note;

                //Remove Candidate
                if (calendarInterview_UserInterviewUpdate.evaluate == 3)
                {
                    if (note.IsNullOrEmpty())
                    {
                        await JS.InvokeVoidAsync("alert", "Mời Nhập Lý Do Loại");
                    }
                    else
                    {
                        CandidateEvaluate.Status = 2;
                        CandidateEvaluate.note_evaluate = note;
                        await httpClient.PutAsJsonAsync("https://localhost:44365/api/candidate/" + CandidateEvaluate.id, CandidateEvaluate);

                        //mail
                        templatemail = templatemails.FirstOrDefault(x => x.id == 7);

                        MailRequest mailRequest = new MailRequest(CandidateEvaluate.Email, templatemail.tittleMail, templatemail.contentMail, CandidateEvaluate.Name);

                        var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
                        await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + calendarInterview_UserInterviewUpdate.id, calendarInterview_UserInterviewUpdate);
                        await JS.InvokeVoidAsync("alert", "Success");
                    }

                }

                //check pass all
                if (calendarInterview_UserInterviewUpdate.evaluate == 2)
                {
                    bool passAll = true;
                    calendarInterview_UserInterviewsUnevaluate.ForEach((x) => { if (x.evaluate != 2) { passAll = false; } });

                    if (passAll)
                    {
                        switch (CandidateEvaluate.Status)
                        {
                            case 6:

                                templatemail = templatemails.FirstOrDefault(x => x.id == 3);
                                CandidateEvaluate.Contacting = 0;
                                CandidateEvaluate.Status = 7;
                                break;
                            case 8:

                                templatemail = templatemails.FirstOrDefault(x => x.id == 4);
                                CandidateEvaluate.Contacting = 0;
                                CandidateEvaluate.Status = 9;
                                break;

                        }
                        await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + CandidateEvaluate.id, CandidateEvaluate);

                        MailRequest mailRequest = new MailRequest(CandidateEvaluate.Email, templatemail.tittleMail, templatemail.contentMail, CandidateEvaluate.Name);

                        var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);

                    }

                    await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview_UserInterview/" + calendarInterview_UserInterviewUpdate.id, calendarInterview_UserInterviewUpdate);
                    await JS.InvokeVoidAsync("alert", "Success");
                }


            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", "Error");

            }
            await getData();
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Close form add tt đánh giá
        /// </summary>
        /// <returns></returns>
        public async Task CloseThis()
        {
            isLoading = true;
            await closeFormthis.InvokeAsync();
            StateHasChanged();
            isLoading = false;
        }

        #endregion functions
    }
}
