using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class FormSendOffer
    {


        #region properties
        [Parameter]
        public Candidate candidateSendOffer { get; set; }
        [Parameter]
        public EventCallback CloseThis { get; set; }
        Confirm_Import_Candidate Confirm_Import_Candidate = new Confirm_Import_Candidate();

        public bool statusLogin { get; set; } = false;

        [Inject] IJSRuntime JS { get; set; }
        //Mail
        public List<TemplateMail> templatemails { get; set; } = new List<TemplateMail> { };
        public TemplateMail templatemail = new TemplateMail();
        public MailRequest mailRequest { get; set; } = new MailRequest();
        //File

        private List<IBrowserFile> loadedFiles = new();
        private long maxFilesize = 999999999999999;
        public bool isLoading { get; set; } = false;

        public string htmlString { get; set; } = string.Empty;
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
            //Mail

            var templateMail = await httpClient.GetAsync("https://localhost:44365/api/TemplateMail");

            templatemails = templateMail.Content.ReadFromJsonAsync<List<TemplateMail>>().Result;


            templatemail = templatemails.FirstOrDefault(x => x.id == 8);


            //content mail
            mailRequest.ToEmail = candidateSendOffer.Email;
            mailRequest.Subject = templatemail.tittleMail;
            mailRequest.Body = templatemail.contentMail;
            //mailRequest.Body = mailRequest.Body.Replace("@nameCandidate", candidateSendOffer.Name);
            mailRequest.Body = mailRequest.Body.Replace("@linkconfirmOffer", "https://localhost:44365/confirmOffer/" + candidateSendOffer.id);

            var rpFormMail = await httpClient.GetAsync("https://localhost:44365/api/TemplateMail/formMail");

            htmlString = rpFormMail.Content.ReadAsStringAsync().Result;

            htmlString = htmlString.Replace("@candidateName", candidateSendOffer.Name);
            htmlString = htmlString.Replace("@contentMail", mailRequest.Body);

            StateHasChanged();
            isLoading = false;



        }

        /// <summary>
        /// - Close form send offer
        /// </summary>
        /// <returns></returns>
        public async Task closeThis()
        {
            isLoading = true;

            await CloseThis.InvokeAsync();
            StateHasChanged();
            isLoading = false;

        }


        /// <summary>
        /// - Update file upload mỗi khi thay đổi
        /// </summary>
        /// <param name="e">Data nhận vào , thay đổi mỗi khi có change event</param>
        /// <returns></returns>
        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            isLoading = true;

            foreach (var file in e.GetMultipleFiles(e.FileCount))
            {
                try
                {
                    string stringbase64 = string.Empty;


                    Stream stream = file.OpenReadStream(999999999999999999);
                    MemoryStream memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    stream.Close();

                    //base64 
                    stringbase64 = Convert.ToBase64String(memoryStream.ToArray());
                    mailRequest.StringFileAttachments.Add(stringbase64);
                    mailRequest.nameFileAttachments.Add(file.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            isLoading = false;

        }

        /// <summary>
        /// - Gửi mail thông tin offer cùng các file thông tin cần thiết
        /// </summary>
        /// <returns></returns>
        public async Task sendOffer()
        {
            isLoading = true;
            try
            {
                //send mail
                var httpClient = new HttpClient();
                mailRequest.Body = mailRequest.Body.Replace("@nameCandidate", candidateSendOffer.Name);
                mailRequest.Body = mailRequest.Body.Replace("@linkconfirmOffer", "https://localhost:44365/confirmOffer/" + candidateSendOffer.id);
                await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);


                //update Candidate
                candidateSendOffer.Status = 10;
                await httpClient.PutAsJsonAsync("https://localhost:44365/api/Candidate/" + candidateSendOffer.id, candidateSendOffer);

                await JS.InvokeVoidAsync("alert", "Gửi Offer Thành Công!");
                await closeThis();
                await getData();
                StateHasChanged();
            }
            catch
            {
                await JS.InvokeVoidAsync("alert", "Gửi Offer Không Thành Công!");
                StateHasChanged();
            }
            isLoading = false;
        }
        #endregion functions
    }
}
