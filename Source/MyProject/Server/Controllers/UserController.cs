using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using MyProject.Server.Repository;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        // GET: api/<UserController>
        [HttpPost("post")]
        public async Task<User> GetDataLoginUser(Login_Information login_Information)
        {
            UserRepo userRepo = new UserRepo();
            User user_login = await userRepo.getUserLogin(login_Information);


            if (user_login.logined == true)
            {
                JWTMiddleware jWTMiddleware = new JWTMiddleware();
                //Get string accessToken
                string accessToken = jWTMiddleware.GenerateJSONWebToken(user_login);
                //Get JwtSecurityToken
                JwtSecurityToken jwtSecurityToken = jWTMiddleware.DecodeJSONWebToken(accessToken);

                user_login.access_token = accessToken;
                user_login.time_access = jwtSecurityToken.Claims.ElementAt(4).Value;
            }
            else
            {
                user_login.access_token = string.Empty;
            }



            return user_login;
        }
        // GET api/<UserController>/5
        [HttpPost("register")]
        public async Task<bool> registerUser(User userRegister)
        {
            try
            {
                UserRepo userRepo = new UserRepo();
                await userRepo.registerUser(userRegister);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("update")]
        public async Task<bool> Put([FromBody] User userUpdate)
        {
            try
            {
                UserRepo userRepo = new UserRepo();
                await userRepo.updateUser(userUpdate);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
