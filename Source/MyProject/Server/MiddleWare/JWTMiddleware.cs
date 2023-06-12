using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MyProject.Shared.Entities
{
    public class JWTMiddleware
    {
        public string GenerateJSONWebToken(User userInfor) 
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetJwtSecret()));

                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> Subject = new List<Claim>() { new Claim("UserID", userInfor.id.ToString()),
                                                          new Claim("UserRole", userInfor.Role.ToString()),
                                                          new Claim("UserName", userInfor.Name),
                                                          new Claim("UserSDT", userInfor.Number_phone.ToString())};


                var tokensecurity = new JwtSecurityToken(
                  AppSettings.GetJwtIssuer(),
                  AppSettings.GetJwtIssuer(),
                  //(IEnumerable<System.Security.Claims.Claim>)userInfo,
                  Subject,
                  expires: DateTime.Now.AddMinutes(60),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokensecurity);

                

                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "false";
            }

        }
        public JwtSecurityToken? DecodeJSONWebToken(string token)
        {
            try 
            {
                JwtSecurityToken jsonSecurityToken = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(token);
                //var asasasa = jsonToken.Claims.ElementAt(0).Value.ToString();
                return jsonSecurityToken;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            
        }
    }
}
