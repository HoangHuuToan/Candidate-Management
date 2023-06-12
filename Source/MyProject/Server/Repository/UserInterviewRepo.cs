 using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;

namespace MyProject.Server.Repository
{
    public class UserInterviewRepo
    {
        public List<UserInterview> lstUserInterview { get; set; } = new List<UserInterview>();
        public List<UserInterview> getUserInterview() 
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM candidate_management.userinterview;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                UserInterview userinterview = new UserInterview(reader.GetInt32("id"),reader.GetString("name_interview"),reader.GetString("email_interview"));

                lstUserInterview.Add(userinterview);

            }
            conn.Close();
            return lstUserInterview;
        }
    }
}
