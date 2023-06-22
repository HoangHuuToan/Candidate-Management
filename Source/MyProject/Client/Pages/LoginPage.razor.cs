using MyProject.Shared.Entities;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using MyProject.Client.LoginManagements;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;

namespace MyProject.Client.Pages
{
    public partial class LoginPage
    {
        public bool isLoading { get; set; } = false;
        public bool isVerified { get; set; } = false;

        public Login_Information loginInfo { get; set; } = new Login_Information();
        public MyProject.Shared.Entities.User user_login { get; set; } = new MyProject.Shared.Entities.User();
        [Inject] IJSRuntime JS { get; set; }
        public bool statusLogin { get; set; } = false;

        public bool isRegister { get; set; } = false;
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
        public async Task Login(KeyboardEventArgs? e = null)
        {

            if (loginInfo.accout_name.IsNullOrEmpty() || loginInfo.accout_password.IsNullOrEmpty())
            {
                await JS.InvokeVoidAsync("alert", "Mời bạn nhập đầy đủ thông tin để đăng nhập !");
            }
            else
            {
                HttpClient httpClient = new HttpClient();

                var IF_login = await httpClient.PostAsJsonAsync("https://localhost:44365/api/User/post", loginInfo);

                user_login = IF_login.Content.ReadFromJsonAsync<User>().Result;



                if (user_login.logined == true)
                {
                    // store data user login to Local Storate
                    await JS.InvokeVoidAsync("setLocalStorate", "data_user_login", user_login);

                    // set status login
                    await JS.InvokeVoidAsync("setLocalStorate", "status_login", true);

                    //Get data in Localstorate
                    string data = await JS.InvokeAsync<string>("getStatusLogin", "data_user_login");

                    //Bind data Js sang Object User C#
                    User dataUserloginLocalStorate = JsonConvert.DeserializeObject<MyProject.Shared.Entities.User>(data);

                    if (user_login.verified == 0)
                    {
                        await JS.InvokeVoidAsync("alert", "Đăng Nhập Thành Công");
                        isVerified = true;
                    }
                    else
                    {
                        await JS.InvokeVoidAsync("LoginSuccess");
                    }


                }
                else
                {
                    await JS.InvokeVoidAsync("setLocalStorate", "status_login", false);
                    await JS.InvokeVoidAsync("alert", "Đăng Nhập Không Thành Công");
                }


            }

        }

        public void registerUser()
        {
            isRegister = true;
        }
        public void closeFormApply()
        {
            isRegister = false;
        }
    }
}
