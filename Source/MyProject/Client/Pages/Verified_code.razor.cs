using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyProject.Shared.Entities;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class Verified_code
    {
        public bool isLoading { get; set; } = false;

        public string code1 { get; set; } = string.Empty;
        public string code2 { get; set; } = string.Empty;
        public string code3 { get; set; } = string.Empty;
        public string code4 { get; set; } = string.Empty;
        public string code5 { get; set; } = string.Empty;
        public string code6 { get; set; } = string.Empty;

        public string codeInsert { get; set; } = string.Empty;

        [Parameter]
        public User userLogin { get; set; } = new User();
        [Inject] IJSRuntime JS { get; set; }
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            await JS.InvokeVoidAsync("ClearAllLocalStorate");
            await getData();
            isLoading = false;
            StateHasChanged();
        }


        /// <summary>
        /// - Get data hiển thị
        /// </summary>
        /// <returns></returns>
        public async Task getData()
        {


        }
        public async Task Check()
        {
            string[] array = new string[] { code1, code2, code3, code4, code5, code6, };


            if (array.Any(x => x == string.Empty))
            {
                await JS.InvokeVoidAsync("alert", "Mời Bạn Nhập Đầy Đủ Mã Xác Nhận");
            }
            else
            {
                codeInsert = code1 + code2 + code3 + code4 + code5 + code6;
                if (codeInsert == userLogin.verified_code)
                {
                    HttpClient httpClient = new HttpClient();

                    userLogin.verified = 1;

                    var IF_register = await httpClient.PutAsJsonAsync("https://localhost:44365/api/User/update", userLogin);

                    await JS.InvokeVoidAsync("alert", "Xác Minh Thành Công !");
                    // store data user login to Local Storate
                    await JS.InvokeVoidAsync("setLocalStorate", "data_user_login", userLogin);

                    // set status login
                    await JS.InvokeVoidAsync("setLocalStorate", "status_login", true);

                    //Get data in Localstorate
                    string data = await JS.InvokeAsync<string>("getStatusLogin", "data_user_login");

                    //Bind data Js sang Object User C#
                    User dataUserloginLocalStorate = JsonConvert.DeserializeObject<MyProject.Shared.Entities.User>(data);

                    await JS.InvokeVoidAsync("LoginSuccess");
                }
                else 
                {
                    await JS.InvokeVoidAsync("alert", "Sai");
                }
            }
        }
    }
}
