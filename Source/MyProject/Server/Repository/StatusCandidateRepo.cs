using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;

namespace MyProject.Server.Repository
{
    
    public class StatusCandidateRepo
    {
        public List<StatusCandidate> statusCandidates { get; set; } = new List<StatusCandidate>();

        public List<StatusCandidate> GetStatusCandidates()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM candidate_management.candidate_status;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                StatusCandidate status = new StatusCandidate(reader.GetInt32("id_status"),reader.GetString("name_status"));

                statusCandidates.Add(status);

            }
            conn.Close();
            return statusCandidates;
        }


    }
}
