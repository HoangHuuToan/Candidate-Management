using Microsoft.AspNetCore.Components;
using MyProject.Shared.Entities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace MyProject.Client.LoginManagements
{
    public class LoginManagement
    {
        public IJSRuntime _JSruntime { get; set; }
        public LoginManagement(IJSRuntime runtime)
        {
            this._JSruntime = runtime;
        }
        public async Task<bool> checkLogin()
        {
            string stt_login = await _JSruntime.InvokeAsync<string>("getStatusLogin", "status_login");

            bool status_Login = !stt_login.IsNullOrEmpty() ? JsonConvert.DeserializeObject<bool>(stt_login) : false;

            return status_Login;
        }

        public async void Logout()
        {
            try
            {
                //Clear data user login LocalStorate 
                await _JSruntime.InvokeVoidAsync("Logout");
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
