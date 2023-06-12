using Microsoft.AspNetCore.Components;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class ContactingCandidate
    {
        #region properties
        public List<Candidate> candidateContacting { get; set; } = new List<Candidate>();

        public List<Candidate> candidateContactingAll { get; set; } = new List<Candidate>();
        public bool viewCV { get; set; } = false;

        public bool checkall { get; set; } = false;

        public string pathViewCV { get; set; } = string.Empty;

        public Candidate candidate { get; set; } = new Candidate();

        public CalendarInterview calendarInterview { get; set; } = new CalendarInterview();

        public List<CalendarInterview> CalendarInterviews { get; set; } = new List<CalendarInterview>();

        public List<TemplateMail> templatemails { get; set; } = new List<TemplateMail> { };
        public List<ValuesOfComb> valuesOfCombs { get; set; } = new List<ValuesOfComb>();

        public int? filterPosition { get; set; } = 0;
        public int? filterRole { get; set; } = 0;

        [Inject] IJSRuntime JS { get; set; }

        public bool isLoading { get; set; } = false;

        public bool statusLogin { get; set; } = false;

        //[Parameter]
        //public int Id { get; set; }
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
        /// - get Data hiển thị 
        /// </summary>0
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            //clear
            CalendarInterviews.Clear();
            candidateContacting.Clear();
            //

            var httpClient = new HttpClient();

            var listCandidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/3");


            var listCandidateTest = await httpClient.GetAsync("https://localhost:44365/api/Candidate/status/4");

            var carlendarTest = await httpClient.GetAsync("https://localhost:44365/api/CalendarInterview/stt/1");

            CalendarInterviews = carlendarTest.Content.ReadFromJsonAsync<List<CalendarInterview>>().Result;

            candidateContactingAll = listCandidateTest.Content.ReadFromJsonAsync<List<Candidate>>().Result;

            listCandidate.Content.ReadFromJsonAsync<List<Candidate>>().Result.ForEach(x => candidateContactingAll.Add(x));

            candidateContacting = candidateContactingAll;
            //mail
            var templateMail = await httpClient.GetAsync("https://localhost:44365/api/TemplateMail");

            templatemails = templateMail.Content.ReadFromJsonAsync<List<TemplateMail>>().Result;
            valuesOfCombs = await httpClient.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:44365/api/ValueOfComb");

            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Cập nhật trạng thái liên hệ của ứng viên đc chọn
        /// </summary>
        /// <param name="e">Data nhận mỗi khi change event</param>
        /// <param name="id">id của Candidate cập nhật</param>
        public void rf(ChangeEventArgs e, int id)
        {
            isLoading = true;
            var abc = candidateContacting.FirstOrDefault(c => c.id == id);

            if (abc != null)
            {
                abc.Contacting = int.Parse(e.Value.ToString());
            }
            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Cập nhật trạng thái checked của ứng viên có đc chọn hay không
        /// </summary>
        /// <param name="e">Data nhận mỗi khi có thay đổi change event</param>
        /// <param name="id">id của Candidate cập nhật</param>
        public void changeChecked(ChangeEventArgs e, int id)
        {
            isLoading = true;
            var candidateContactingChange = candidateContacting.FirstOrDefault(c => c.id == id);

            if (candidateContactingChange != null)
            {
                candidateContactingChange.Checked = !candidateContactingChange.Checked;
            }

            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Thực hiện change check all
        /// </summary>
        public void changeCheckedAll()
        {
            isLoading = true;
            candidateContacting.ForEach(x => x.Checked = !checkall);
            checkall = !checkall;

            StateHasChanged();
            isLoading = false;
        }

        /// <summary>
        /// - Gửi mail cho ứng viên hẹn lịch test
        /// </summary>
        /// <param name="id">id của Candidate gửi mail</param>
        /// <returns></returns>
        public async Task sendMail(int id)
        {
            isLoading = true;
            var candidateContactingTmp = candidateContacting.FirstOrDefault(x => x.id == id);
            if (candidateContactingTmp != null)
            {
                candidate = candidateContactingTmp;
            }


            var httpClient = new HttpClient();
            TemplateMail templatemail = new TemplateMail();


            var templatemailTmp = templatemails.FirstOrDefault(x => x.id == 1);

            if (templatemailTmp != null)
            {
                templatemail = templatemailTmp;
            }


            MailRequest mailRequest = new MailRequest(candidate.Email, templatemail.tittleMail, templatemail.contentMail + "<br/> Thời Gian :" + candidate.timeInterview + "<br/> Địa Điểm :" + candidate.addressInterview, candidate.Name);

            var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
            isLoading = false;
        }


        /// <summary>
        /// - Gửi mail loại cho ứng viên bị loại
        /// </summary>
        /// <param name="id">id của Candidate gửi nhận mail </param>
        /// <returns></returns>
        public async Task sendMailRemove(int id)
        {
            isLoading = true;
            candidate = candidateContacting.FirstOrDefault(x => x.id == id);

            var httpClient = new HttpClient();
            TemplateMail templatemail = new TemplateMail();

            templatemail = templatemails.FirstOrDefault(x => x.id == 7);

            MailRequest mailRequest = new MailRequest(candidate.Email, templatemail.tittleMail, templatemail.contentMail, candidate.Name);

            var sendMail = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
            isLoading = false;
        }

        /// <summary>
        /// - Update thông tin lịch pv và trạng thái của Candidate
        /// </summary>
        /// <param name="id">id của Candidate cần cập nhật</param>
        /// <returns></returns>
        public async Task update(int id)
        {
            isLoading = true;
            if (candidateContacting.FirstOrDefault((x) => x.id == id).addressInterview.IsNullOrEmpty())
            {
                await JS.InvokeVoidAsync("alert", "Error! Nhập đầy đủ thông tin");
            }
            else
            {
                try
                {
                    var httpClient = new HttpClient();

                    Candidate candidate = new Candidate();
                    candidate = candidateContacting.FirstOrDefault(c => c.id == id);

                    if (candidate.gradeTest.IsNullOrEmpty())
                    {
                        CalendarInterview calendarItv = new CalendarInterview("Mời Test", candidate.id, candidate.timeInterview, candidate.addressInterview);
                        if (candidate.Status >= 4)
                        {
                            calendarItv.timeInterview = CalendarInterviews.FirstOrDefault(x => x.idCandidate == candidate.id).timeInterview;
                            calendarItv.addressInterview = CalendarInterviews.FirstOrDefault(x => x.idCandidate == candidate.id).addressInterview;
                        }
                        if (CalendarInterviews.Exists(x => x.nameInterview == calendarItv.nameInterview && x.idCandidate == calendarItv.idCandidate))
                        {
                            await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarItv);
                        }
                        else
                        {
                            await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview/add", calendarItv);
                        }
                        await sendMail(candidate.id);
                        candidate.Status = 4;
                        StateHasChanged();
                    }
                    if (!candidate.gradeTest.IsNullOrEmpty() && int.Parse(candidate.gradeTest) >= 5)
                    {
                        candidate.Status = 5;
                        candidate.Contacting = 0;
                        StateHasChanged();
                    }
                    else if (!candidate.gradeTest.IsNullOrEmpty() && int.Parse(candidate.gradeTest) < 5)
                    {
                        await sendMailRemove(candidate.id);
                        candidate.Status = 2;
                        candidate.Contacting = 0;
                        StateHasChanged();
                    }
                    var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + id, candidate);
                    await getData();
                    await JS.InvokeVoidAsync("alert", "Update Success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await JS.InvokeVoidAsync("alert", "Update Error!");
                }
                StateHasChanged();

            }
            isLoading = false;
        }


        /// <summary>
        /// -Update tất cả các ứng viên được chọn
        /// </summary>
        /// <returns></returns>
        public async Task updateAll()
        {
            isLoading = true;
            var httpClient = new HttpClient();

            foreach (var candidatect in candidateContacting)
            {
                if (candidatect.Checked == true)
                {

                    if (!candidatect.addressInterview.IsNullOrEmpty() && candidate.gradeTest.IsNullOrEmpty())
                    {
                        CalendarInterview calendarItv = new CalendarInterview("Mời Test", candidatect.id, candidatect.timeInterview, candidatect.addressInterview);
                        if (CalendarInterviews.Exists(x => x.nameInterview == calendarItv.nameInterview && x.idCandidate == calendarItv.idCandidate))
                        {
                            await httpClient.PutAsJsonAsync("https://localhost:44365/api/CalendarInterview", calendarItv);
                        }
                        else
                        {
                            await httpClient.PostAsJsonAsync("https://localhost:44365/api/CalendarInterview/add", calendarItv);
                        }
                        sendMail(candidatect.id);
                        candidatect.Status = 4;
                        StateHasChanged();
                    }
                    if (!candidatect.gradeTest.IsNullOrEmpty() && int.Parse(candidatect.gradeTest) >= 5)
                    {
                        candidatect.Status = 5;
                        candidatect.Contacting = 0;
                        StateHasChanged();

                    }
                    else if (!candidatect.gradeTest.IsNullOrEmpty() && int.Parse(candidatect.gradeTest) < 5)
                    {
                        sendMailRemove(candidate.id);
                        candidatect.Status = 2;
                        candidatect.Contacting = 0;
                        StateHasChanged();
                    }
                    var re = await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidatect.id, candidatect);
                }
            }
            await getData();
            StateHasChanged();
            isLoading = false;
        }

        #region search-filter
        /// <summary>
        /// - Tìm kiếm ứng viên trong danh sách bằng tên
        /// </summary>
        /// <param name="txt_search">Chuỗi nhận vào để search</param>
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
                candidateContacting = candidateContacting.FindAll(x => x.Name.ToLower().Trim().Contains(searchKeyword));
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
        /// - Lọc ứng viên theo vị trí
        /// </summary>
        /// <param name="positionFilter">id vị trí cần lọc</param>
        public void filterPositions(string positionFilter)
        {
            isLoading = true;
            filterPosition = Int32.Parse(positionFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateContacting = candidateContactingAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }

        /// <summary>
        /// - Lọc ứng viên theo chức danh
        /// </summary>
        /// <param name="roleFilter">id chức danh cần lọc</param>
        public void filterRoles(string roleFilter)
        {
            isLoading = true;
            filterRole = Int32.Parse(roleFilter.ToString());
            if (filterRole == 0 && filterPosition == 0)
            {
                candidateContacting = candidateContactingAll;
            }
            if (filterRole != 0 && filterPosition != 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Role == filterRole && x.Position == filterPosition);
            }
            if (filterRole != 0 && filterPosition == 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Role == filterRole);
            }
            if (filterRole == 0 && filterPosition != 0)
            {
                candidateContacting = candidateContactingAll.FindAll(x => x.Position == filterPosition);
            }
            isLoading = false;
        }
        #endregion search-filter
        #endregion functions

    }
}
