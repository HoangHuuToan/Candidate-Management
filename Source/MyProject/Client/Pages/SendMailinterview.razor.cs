using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MyProject.Client.Pages
{
    public partial class SendMailinterview
    {

        #region properties
        public List<Candidate> candidateSendMails { get; set; } = new List<Candidate>();

        public List<Candidate> candidateContactingAll { get; set; } = new List<Candidate>();

        public bool checkall { get; set; } = false;

        public List<CalendarInterview> CalendarInterviewPVV1 { get; set; } = new List<CalendarInterview>();
        public List<CalendarInterview> CalendarInterviewPVV2 { get; set; } = new List<CalendarInterview>();
        public List<CalendarInterview> CalendarInterviewAll { get; set; } = new List<CalendarInterview>();
        public bool prvMail { get; set; } = false;


        public Candidate candidateprvMail { get; set; } = new Candidate();

        //Mail

        public TemplateMail mailCurrently { get; set; } = new TemplateMail();

        public List<TemplateMail> templatemails { get; set; } = new List<TemplateMail> { };
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();
        public int typeMail { get; set; }
        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;

        [Inject] IJSRuntime JS { get; set; }
        public bool statusLogin { get; set; } = false;

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
            var httpClient = new HttpClient();


            var listCandidateSendMailv1 = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/5");

            //var listCandidateSentv1 = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/6");

            var listCandidateSendMailv2 = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/7");

            //var listCandidateSentv2 = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/8");

            //Add data Candidate

            candidateContactingAll = listCandidateSendMailv1.Content.ReadFromJsonAsync<List<Candidate>>().Result;
            listCandidateSendMailv2.Content.ReadFromJsonAsync<List<Candidate>>().Result.ForEach(x => candidateContactingAll.Add(x));


            candidateSendMails = candidateContactingAll;



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
        /// - update trạng thái liên hệ của ứng viên mỗi khi thay đổi
        /// </summary>
        /// <param name="e">Data nhận sẽ cập nhật mỗi khi thay đổi</param>
        /// <param name="id">id của ứng viên cập nhật</param>
        public void rf(ChangeEventArgs e, int id)
        {
            isLoading = true;
            candidateSendMails.FirstOrDefault(c => c.id == id).Contacting = int.Parse(e.Value.ToString());
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - THay đổi trạng thái checked khi có sự thay đổi
        /// </summary>
        /// <param name="e">Data nhận sẽ cập nhật mỗi khi thay đổi</param>
        /// <param name="id">id của ứng viên cập nhật</param>
        public void changeChecked(ChangeEventArgs e, int id)
        {
            isLoading = true;
            candidateSendMails.FirstOrDefault(c => c.id == id).Checked = !candidateSendMails.FirstOrDefault(c => c.id == id).Checked;
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Cloese component Prv Email
        /// </summary>
        public void CloseprvMailCpn()
        {
            isLoading = true;
            getData();
            prvMail = false;
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Hiển thị component prv mail
        /// </summary>
        /// <param name="id">id Candidate prv Mail</param>
        public void prvMailCpn(int id)
        {
            isLoading = true;
            prvMail = true;

            candidateprvMail = candidateSendMails.FirstOrDefault(x => x.id == id);

            switch (candidateprvMail.Status)
            {
                case 5:
                    typeMail = 3;
                    break;

                case 7:
                    typeMail = 4;
                    break;
                default:
                    break;

            }

            mailCurrently = templatemails.FirstOrDefault(x => x.id == typeMail);
            //candidateprvMail.timeInterview = "";
            //candidateprvMail.addressInterview = "";
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// Thực hiển chọn / bỏ tất cả
        /// </summary>
        public void changeCheckedAll()
        {
            isLoading = true;
            candidateSendMails.ForEach(x => x.Checked = !checkall);
            checkall = !checkall;

            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Thực hiện tạo lịch
        /// </summary>
        /// <returns></returns>
        public async Task CreateCalendar()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            try
            {
                foreach (var candidate in candidateSendMails)
                {
                    if (candidate.Checked == true)
                    {

                        if (!candidate.addressInterview.IsNullOrEmpty())
                        {
                            CalendarInterview calendarItv = new CalendarInterview();
                            if (candidate.Status == 5)
                            {
                                calendarItv = new CalendarInterview("PV V1", candidate.id, candidate.timeInterview, candidate.addressInterview);
                            }

                            if (candidate.Status == 7)
                            {
                                calendarItv = new CalendarInterview("PV V2", candidate.id, candidate.timeInterview, candidate.addressInterview);
                            }

                            bool add = true;
                            foreach (var calendar in CalendarInterviewAll)
                            {
                                if (calendarItv.idCandidate == calendar.idCandidate && calendarItv.nameInterview == calendar.nameInterview)
                                {
                                    await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarItv);
                                    add = false;
                                    break;
                                }
                            }
                            if (add)
                            {
                                await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview/add", calendarItv);

                            }
                            //candidate.Status = 6;
                            StateHasChanged();
                        }

                        var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidate.id, candidate);
                    }
                }
                await getData();
                await JS.InvokeVoidAsync("alert", " Thành công!");
                StateHasChanged();

            }
            catch
            {
                await JS.InvokeVoidAsync("alert", " Không Thành công!");

            }

            isLoading = false;

        }


        /// <summary>
        /// - Thực hiện gửi mail
        /// </summary>
        /// <returns></returns>
        public async Task sendMail()
        {
            isLoading = true;
            try
            {
                var httpClient = new HttpClient();
                foreach (var candidate in candidateSendMails)
                {
                    if (candidate.Checked == true)
                    {
                        CalendarInterview calendarItv = new CalendarInterview();
                        if (candidate.Status == 5)
                        {
                            calendarItv = new CalendarInterview("PV V1", candidate.id, candidate.timeInterview, candidate.addressInterview);
                        }

                        if (candidate.Status == 7)
                        {
                            calendarItv = new CalendarInterview("PV V2", candidate.id, candidate.timeInterview, candidate.addressInterview);
                        }
                        bool add = true;
                        foreach (var calendar in CalendarInterviewAll)
                        {
                            if (calendarItv.idCandidate == calendar.idCandidate && calendarItv.nameInterview == calendar.nameInterview)
                            {
                                await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarItv);
                                add = false;
                                break;
                            }
                        }
                        if (add)
                        {
                            await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview/add", calendarItv);

                        }

                        TemplateMail templatemail = new TemplateMail();
                        switch (candidate.Status)
                        {
                            case 5:

                                templatemail = templatemails.FirstOrDefault(x => x.id == 3);
                                //candidate.Contacting = 0;
                                candidate.Status = 6;
                                break;
                            case 7:

                                templatemail = templatemails.FirstOrDefault(x => x.id == 3);
                                //candidate.Contacting = 0;
                                candidate.Status = 8;
                                break;

                        }

                        MailRequest mailRequest = new MailRequest(candidate.Email, templatemail.tittleMail, templatemail.contentMail + "<br/> Thời Gian :" + candidate.timeInterview + "<br/> Địa Điểm :" + candidate.addressInterview, candidate.Name);

                        var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);


                    }

                    var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidate.id, candidate);

                }

                await getData();
                await JS.InvokeVoidAsync("alert", "Success");
                StateHasChanged();



            }
            catch (Exception e)
            {
                await JS.InvokeVoidAsync("alert", "Error");
            }
            isLoading = false;

        }

        #region search-filter

        /// <summary>
        /// - Tìm kiếm dánh sách theo tên
        /// </summary>
        /// <param name="txt_search">Chuôi tìm kiếm nhập vào</param>
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
                candidateSendMails = candidateSendMails.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
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
        /// - Lọc danh sách theo Ngôn Ngữ
        /// </summary>
        /// <param name="positionFilter">id Ngôn Ngữ cần lọc</param>
        public void filterPositions(string positionFilter)
        {
            isLoading = true;
            filterPosition = Int32.Parse(positionFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateSendMails = candidateContactingAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }

        /// <summary>
        /// - Lọc danh sách theo Vị Trí Ứng Tuyển
        /// </summary>
        /// <param name="roleFilter">id Vị Trí Ứng Tuyển cần lọc</param>
        public void filterRoles(string roleFilter)
        {
            isLoading = true;
            filterRole = Int32.Parse(roleFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateSendMails = candidateContactingAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateSendMails = candidateContactingAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }
        #endregion search-filter
        #endregion functions
    }
}
