using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{
    public partial class Register
    {
        public User user { get; set; } = new User();

        [Parameter]
        public EventCallback Close { get; set; }
        public bool statusLogin { get; set; } = false;
        public bool isLoading { get; set; } = false;

        [Inject] IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {


            StateHasChanged();

        }


        /// <summary>
        /// - get Data hiển thị 
        /// </summary>0
        /// <returns></returns>
        public async Task getData()
        {
            isLoading = true;
            HttpClient httpClient = new HttpClient();
            isLoading = false;
            StateHasChanged();
        }
        public void CloseThis()
        {
            Close.InvokeAsync();
            StateHasChanged();
        }

        public async Task registerUser()
        {
            try
            {
                if (user.Name.IsNullOrEmpty() || user.accout_name.IsNullOrEmpty() || user.accout_password.IsNullOrEmpty() || user.Address.IsNullOrEmpty() || user.Number_phone <= 0 || user.Email.IsNullOrEmpty()) 
                {
                    await JS.InvokeVoidAsync("alert", "Mời bạn nhập đầy đủ thông tin để đăng ký !");
                }
                else { 
                HttpClient httpClient = new HttpClient();

                Random random = new Random();
                int tmp = random.Next(000000, 999999) + 1000000;
                string verifi_code = tmp.ToString().Substring(1, 6);
                user.verified_code = verifi_code;

                var IF_register = await httpClient.PostAsJsonAsync("https://localhost:44365/api/User/register", user);

                MailRequest mailRequest = new MailRequest(user.Email, "Verifi Code Accout", "Đây là mã xác thực tài khoản của bạn <b>" + user.verified_code + "</b>", user.Name);
                await httpClient.PostAsJsonAsync("https://localhost:44365/api/Sendmail", mailRequest);
                await JS.InvokeVoidAsync("alert","Bạn đã đăng ký thành công vui lòng kiểm tra mail để lấy mã xác thực !");
                closeFormApply();
                StateHasChanged();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                await JS.InvokeVoidAsync("alert", "Bạn đã đăng ký không thành công vui lòng kiểm tra lại thông tin !");
            }
            StateHasChanged();
        }
        public async void closeFormApply() 
        {
            await Close.InvokeAsync();
            StateHasChanged();
        }
    }
}
