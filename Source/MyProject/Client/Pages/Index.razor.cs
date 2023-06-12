using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MyProject.Client.LoginManagements;
using MyProject.Shared.Entities;
using Newtonsoft.Json;

namespace MyProject.Client.Pages
{
    public partial class Index
    {
        public bool isLoading { get; set; } = false;
        public bool statusLogin { get; set; } = false;
        public User dataUserloginLocalStorate { get; set; } = new User();

        [Inject] IJSRuntime JS { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;
                await checkTimeLogin();
                LoginManagement loginManagement = new LoginManagement(JS);
                await getData();
                statusLogin = await loginManagement.checkLogin();
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void Logout()
        {
            LoginManagement loginManagement = new LoginManagement(JS);
            loginManagement.Logout();
        }
        public async Task getData()
        {
            isLoading = true;
            //Get data in Localstorate
            string data = await JS.InvokeAsync<string>("getStatusLogin", "data_user_login");

            if (!data.IsNullOrEmpty())
            {
                dataUserloginLocalStorate = JsonConvert.DeserializeObject<User>(data);
            }
            //Bind data Js sang Object User C#
            isLoading = false;
        }

        public async Task checkTimeLogin()
        {
            //get time Stamp now
            LoginManagement loginManagementN = new LoginManagement(JS);
            var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            await getData();

            if (Int32.Parse(dataUserloginLocalStorate.time_access) < Int32.Parse(timeStamp))
            {
                isLoading = true;
                await JS.InvokeVoidAsync("ClearAllLocalStorate");
                statusLogin = await loginManagementN.checkLogin();
                isLoading = false;
                StateHasChanged();
            }
            
        }
    }
}
