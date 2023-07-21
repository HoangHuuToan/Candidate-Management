using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MyProject.Server.Controllers;
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

            MySqlCommand command = new MySqlCommand("SELECT * FROM candidate_management.user " +
                                                                                                        " WHERE accout_name = @accout_name " +
                                                                                                              " and accout_password = @accout_password ;", conn);

            command.Parameters.Add("@accout_name", MySqlDbType.VarChar).Value = login_Information.accout_name;
            command.Parameters.Add("@accout_password", MySqlDbType.VarChar).Value = login_Information.accout_password;

            MySqlDataReader reder = command.ExecuteReader();


            List<User> login_ifs = new List<User>();




            while (reder.Read())
            {
                User login_user = new User();
                login_user.id = reder.GetInt32("id");
                login_user.Name = reder.GetString("name");
                login_user.birth_day = reder.GetDateTime("birth_day");
                login_user.Address = reder.GetString("address");
                login_user.Number_phone = reder.GetInt32("number_phone");
                login_user.Email = reder.GetString("email");

                login_user.Role = reder.GetInt32("role");
                login_user.accout_name = reder.GetString("accout_name");
                login_user.accout_password = reder.GetString("accout_password");
                login_user.verified = reder["verified"] == DBNull.Value ? 0 : reder.GetInt32("verified");
                login_user.verified_code = reder["verifi_code"] == DBNull.Value ? "" : reder.GetString("verifi_code");


                login_ifs.Add(login_user);

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
                    userLogin = login_ifs.FirstOrDefault();
                    userLogin.logined = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            }

            return userLogin;
        }
        public async Task registerUser(User userRegister)
        {
            try
            {
                MySqlConnection conn = AppSettings.GetConnection();
                conn.Open();

                //insert data user
                string sql1 = "INSERT INTO `candidate_management`.`user` (name, birth_day, address, number_phone, email, role, accout_name, accout_password, verified,verifi_code) VALUES (@name, @birth_day, @address, @number_phone, @email, 1, @accout_name, @accout_password, 0,@verifi_code);";
                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

                cmd1.Parameters.Add("@name", MySqlDbType.VarChar).Value = userRegister.Name;

                //cmd1.Parameters.Add("@birth_day", MySqlDbType.Date).Value = userRegister.birth_day;
                cmd1.Parameters.Add("@birth_day", MySqlDbType.Date).Value = userRegister.birth_day;
                cmd1.Parameters.Add("@address", MySqlDbType.VarChar).Value = userRegister.Address;
                cmd1.Parameters.Add("@number_phone", MySqlDbType.Int32).Value = userRegister.Number_phone;
                cmd1.Parameters.Add("@email", MySqlDbType.VarChar).Value = userRegister.Email;

                cmd1.Parameters.Add("@accout_name", MySqlDbType.VarChar).Value = userRegister.accout_name;
                cmd1.Parameters.Add("@accout_password", MySqlDbType.VarChar).Value = userRegister.accout_password;
                cmd1.Parameters.Add("@verifi_code", MySqlDbType.VarChar).Value = userRegister.verified_code;

                cmd1.ExecuteNonQuery();



                conn.Close();
                //SendmailController sendmailController = new SendmailController();
                //

                //sendmailController.Post(mailRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }


        }

        public async Task updateUser(User userRegister)
        {
            try
            {
                MySqlConnection conn = AppSettings.GetConnection();
                conn.Open();
                string sql1 = "UPDATE `candidate_management`.`user` SET name = @name_user ," +
                                                                        " birth_day = @birth_day , " +
                                                                        " address = @address ," +
                                                                        " number_phone = @number_phone ," +
                                                                        " email = @email ," +
                                                                        " role = @role ," +
                                                                        " accout_name = @accout_name ," +
                                                                        " accout_password = @accout_password ," +
                                                                        " verified = @verified ," +
                                                                        " verifi_code = @verifi_code " +
                                                                        " WHERE id = @id ;";
                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

                cmd1.Parameters.Add("@name_user", MySqlDbType.VarChar).Value = userRegister.Name;
                cmd1.Parameters.Add("@birth_day", MySqlDbType.Date).Value = userRegister.birth_day;
                cmd1.Parameters.Add("@address", MySqlDbType.VarChar).Value = userRegister.Address;
                cmd1.Parameters.Add("@number_phone", MySqlDbType.Int32).Value = userRegister.Number_phone;
                cmd1.Parameters.Add("@email", MySqlDbType.VarChar).Value = userRegister.Email;
                cmd1.Parameters.Add("@role", MySqlDbType.Int32).Value = userRegister.Role;
                cmd1.Parameters.Add("@accout_name", MySqlDbType.VarChar).Value = userRegister.accout_name;
                cmd1.Parameters.Add("@accout_password", MySqlDbType.VarChar).Value = userRegister.accout_password;
                cmd1.Parameters.Add("@verified", MySqlDbType.Int32).Value = userRegister.verified;
                cmd1.Parameters.Add("@verifi_code", MySqlDbType.VarChar).Value = userRegister.verified_code;

                cmd1.Parameters.Add("@id", MySqlDbType.Int32).Value = userRegister.id;

                cmd1.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

    }
}
