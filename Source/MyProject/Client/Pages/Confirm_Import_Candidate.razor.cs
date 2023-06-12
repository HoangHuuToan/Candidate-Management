using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Graph.Models;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using System.Net.Http.Json;
using MyProject.Client.LoginManagements;

namespace MyProject.Client.Pages
{
    public partial class Confirm_Import_Candidate
    {


        #region properties

        [Parameter]
        public Candidate candidate_insert { get; set; }

        [Parameter]
        public EventCallback closeConfirm { get; set; }

        public List<ValuesOfComb> valuesOfCombs = new List<ValuesOfComb>();
        public string stringbase64 { get; set; }
        [Inject] IJSRuntime JS { get; set; }

        public List<Candidate> lstCandidateExits { get; set; } = new List<Candidate>();
        public bool isLoading { get; set; } = false;


        public bool statusLogin { get; set; } = false;
        #endregion properties

        #region Functions


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
                //var response = await client.GetFromJsonAsync<List<ValuesOfComb>>("https://localhost:7199/api/ValueOfComb");

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44365/api/ValueOfComb"))
                    {
                        valuesOfCombs = response.Content.ReadFromJsonAsync<List<ValuesOfComb>>().Result;
                    }
                }
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
        /// - Thêm ứng viên
        /// </summary>
        /// <returns></returns>
        public async Task importCandidate()
        {
            isLoading = true;
            Candidate candidate = new Candidate(
                                                candidate_insert.strID,
                                                candidate_insert.Name,
                                                candidate_insert.Role,
                                                candidate_insert.Position,
                                                candidate_insert.BirthDay,
                                                candidate_insert.Address,
                                                candidate_insert.NumberPhone,
                                                candidate_insert.Email,
                                                candidate_insert.pathCV,
                                                candidate_insert.Origin,
                                                candidate_insert.Contacting,
                                                candidate_insert.Applied,
                                                candidate_insert.Status,
                                                candidate_insert.gradeTest,
                                                candidate_insert.Note,
                                                candidate_insert.FlagDlt,
                                                candidate_insert.strBase64pdf);
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var listCandidate = await httpClient.GetAsync("https://localhost:44365/api/Candidate");

                    lstCandidateExits = listCandidate.Content.ReadFromJsonAsync<List<Candidate>>().Result;

                    foreach (Candidate candd in lstCandidateExits)
                    {
                        if (candidate.Name == candd.Name && candidate.Email == candd.Email && candidate.NumberPhone == candd.NumberPhone && candidate.BirthDay == candd.BirthDay)
                        {
                            candidate.Applied = 1;
                            break;
                        }
                    }

                    var response = await httpClient.PostAsJsonAsync("https://localhost:44365/api/Candidate/add", candidate);
                    await JS.InvokeVoidAsync("ShowMessaggeSuccess", "Nhập Ứng Viên Thành Công");
                    await closeConfirm.InvokeAsync();

                }
                catch (Exception e)
                {
                    await JS.InvokeVoidAsync("ShowMessaggeError", "Nhập Ứng Viên Thất Bại");
                    throw;
                }
                isLoading = false;
            }


        }
        #endregion Functions
    }
}
