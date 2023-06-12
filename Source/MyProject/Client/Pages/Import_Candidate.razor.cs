using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Kiota.Abstractions;
using MyProject.Shared.Entities;
using System.Collections;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MyProject.Shared.Path;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;

namespace MyProject.Client.Pages
{


    public partial class Import_Candidate
    {

        #region properties
        public bool show_Cpn_Confirm { get; set; } = false;

        public List<ValuesOfComb> valuesOfCombs = new List<ValuesOfComb>();
        public string stringbase64 { get; set; }

        [Inject] IJSRuntime JS { get; set; }
        public bool statusLogin { get; set; } = false;
        Candidate candidate = new();

        Confirm_Import_Candidate Confirm_Import_Candidate = new Confirm_Import_Candidate();

        //File
        private long maxFilesize = 999999999999999;
        private int maxAllowedFiles = 1;

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
            //var response = await client.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:7199/api/ValueOfComb");
            LoginManagement loginManagement = new LoginManagement(JS);
            statusLogin = await loginManagement.checkLogin();
            if (statusLogin)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44365/api/ValueOfComb"))
                    {
                        valuesOfCombs = response.Content.ReadFromJsonAsync<List<ValuesOfComb>>().Result;
                    }
                }
                //StateHasChanged();

                candidate.Position = valuesOfCombs.FirstOrDefault(x => x.valueOfComb == 1).id;
                candidate.Role = valuesOfCombs.FirstOrDefault(x => x.valueOfComb == 2).id;
                candidate.Origin = valuesOfCombs.FirstOrDefault(x => x.valueOfComb == 3).id;
            }
            else
            {
                await JS.InvokeVoidAsync("Logout");
            }
            isLoading = false;
            StateHasChanged();
        }

        /// <summary>
        /// - Clear data Candidate import
        /// </summary>
        public void clear()
        {
            isLoading = true;
            candidate = new Candidate();
            isLoading = false;
        }

        /// <summary>
        /// - Hiển thị cpn confirm thông tin Candidate muốn add
        /// </summary>
        /// <returns></returns>
        public async Task show_importCandidate()
        {
            isLoading = true;
            if (candidate.Name.IsNullOrEmpty() || candidate.NumberPhone.ToString().IsNullOrEmpty() || candidate.Email.IsNullOrEmpty() || candidate.Address.IsNullOrEmpty() || candidate.pathCV.IsNullOrEmpty())
            {
                await JS.InvokeVoidAsync("alert", "Mời Nhập Đầy Đủ Thông Tin Ứng Viên");
            }
            else
            {
                show_Cpn_Confirm = true;
            }
            isLoading = false;
        }

        /// <summary>
        /// - Đóng form confirm tt candidate import
        /// </summary>
        public void closeCF()
        {
            isLoading = true;
            show_Cpn_Confirm = false;
            clear();
            StateHasChanged();
            isLoading = false;
        }


        /// <summary>
        /// - Load file, cập nhật file CV của ứng viên
        /// </summary>
        /// <param name="e">Data cập nhật mỗi khi có change event</param>
        /// <returns></returns>
        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            isLoading = true;
            candidate.strID = "VN" + Guid.NewGuid();
            var loadedFile1 = e.GetMultipleFiles().FirstOrDefault();
            try
            {

                var a = loadedFile1.Name;
                var b = Path.GetExtension(loadedFile1.Name);
                string newFileName = Path.ChangeExtension(candidate.Name + candidate.strID + "_CV", Path.GetExtension(loadedFile1.Name));
                candidate.pathCV = link.Path_Folder_CV + newFileName;

                Stream stream = loadedFile1.OpenReadStream(99999999999);
                MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                stream.Close();
                //mảng byte
                //var op = memoryStream.ToArray();

                //base64 
                stringbase64 = Convert.ToBase64String(memoryStream.ToArray());
                candidate.strBase64pdf = stringbase64;
                isLoading = false;
            }
            catch (Exception ex)
            {
            }


        }
        #endregion functions
    }



}
