using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;
using System.Data;

namespace MyProject.Server.Repository
{
    public class UserRepo
    {

        [Inject] IJSRuntime JS { get; set; }

        public async Task<User> getUserLogin(Login_Information login_Information)
        {
            User userLogin = new User();

            MySqlConnection conn = AppSettings.GetConnection();
            conn.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM candidate_management.login_information " +
                                                                                                        " WHERE accout_name = @accout_name " +
                                                                                                              " and accout_password = @accout_password ;", conn);

            command.Parameters.Add("@accout_name", MySqlDbType.VarChar).Value = login_Information.accout_name;
            command.Parameters.Add("@accout_password", MySqlDbType.VarChar).Value = login_Information.accout_password;

            MySqlDataReader reder = command.ExecuteReader();


            List<Login_Information> login_ifs = new List<Login_Information>();




            while (reder.Read())
            {
                Login_Information login_InformationTmp = new Login_Information();
                login_InformationTmp.id = reder.GetInt32("id");
                login_InformationTmp.id_user = reder.GetInt32("id_user");
                login_InformationTmp.accout_name = reder.GetString("accout_name");
                login_InformationTmp.accout_password = reder.GetString("accout_password");
                login_InformationTmp.verified = reder.GetInt32("verified");
                login_InformationTmp.verified_code = reder["verified_code"] == DBNull.Value ? "" : reder.GetString("verified_code");


                login_ifs.Add(login_InformationTmp);

            }
            conn.Close();

            if (login_ifs.Count < 1)
            {
                userLogin.logined = false;
            }
            else if (login_ifs.Count > 1)
            {
                userLogin.logined = false;
            }
            else if (login_ifs.Count == 1)
            {

                try
                {
                    conn.Open();
                    MySqlCommand command2 = new MySqlCommand("SELECT * FROM candidate_management.user WHERE user.id = @id_user;", conn);
                    command2.Parameters.Add("@id_user", MySqlDbType.Int32).Value = login_ifs.FirstOrDefault().id_user;
                    MySqlDataReader reder2;
                    reder2 = command2.ExecuteReader();

                    while (reder2.Read())
                    {
                        userLogin.id = reder2.GetInt32("id");
                        userLogin.Name = reder2.GetString("name");
                        userLogin.Address = reder2.GetString("address");
                        userLogin.Number_phone = reder2.GetInt32("number_phone");
                        userLogin.Email = reder2.GetString("email");
                        userLogin.Role = reder2.GetInt32("role");
                        userLogin.verified = login_ifs.FirstOrDefault().verified;
                        userLogin.verified_code = login_ifs.FirstOrDefault().verified_code;
                        userLogin.logined = true;
                    }
                    //await JS.InvokeVoidAsync("localStorage.setItem", "status_login", true);
                    //await JS.InvokeVoidAsync("eval", "localStorage.setItem("status_login',"Đã xóa lịch phỏng vấn này")");
                    conn.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            }

            return userLogin;
        }
    }
}
