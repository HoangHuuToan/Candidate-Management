using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;

namespace MyProject.Server.Repository
{
    public class CalendarInterview_UserInterviewRepo
    {

        public List<CalendarInterview_UserInterview> calendarInterview_UserInterviews { get; set; } = new List<CalendarInterview_UserInterview>();
        public bool Add(CalendarInterview_UserInterview calendarInterview_UserInterview)
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "INSERT INTO `candidate_management`.`calendarinterview_userinterview` (`id_calendarinterview`, `id_userinterview`) VALUES ( @id_calendarinterview , @id_userinterview );";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("id_calendarinterview", MySqlDbType.Int32).Value = calendarInterview_UserInterview.id_calendarinterview;
            cmd.Parameters.Add("id_userinterview", MySqlDbType.Int32).Value = calendarInterview_UserInterview.id_userinterview;

            cmd.ExecuteNonQuery();

            conn.Close();

            return true;
        }

        public bool Update(CalendarInterview_UserInterview calendarInterview_UserInterview)
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "update candidate_management.calendarinterview_userinterview SET id_calendarinterview = @id_calendarinterview, id_userinterview = @id_userinterview , evaluate = @evaluate, note_evaluate = @note_evaluate   WHERE id = @id ;";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                // Create and set the parameters values 
                //cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = candidate.id;
                cmd.Parameters.Add("@id_calendarinterview", MySqlDbType.Int32).Value = calendarInterview_UserInterview.id_calendarinterview;
                cmd.Parameters.Add("@id_userinterview", MySqlDbType.Int32).Value = calendarInterview_UserInterview.id_userinterview;
                cmd.Parameters.Add("@evaluate", MySqlDbType.Int32).Value = calendarInterview_UserInterview.evaluate;
                cmd.Parameters.Add("@note_evaluate", MySqlDbType.String).Value = calendarInterview_UserInterview.note_evaluate;


                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = calendarInterview_UserInterview.id; //where

                // Let's ask the db to execute the query
                cmd.ExecuteNonQuery();


            }
            conn.Close();

            return true;
        }
        public List<CalendarInterview_UserInterview> getAllCalendar_User()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "SELECT calendarinterview_userinterview.*,j1.name_interview as 'name_User' FROM candidate_management.calendarinterview_userinterview inner join userinterview as j1 on calendarinterview_userinterview.id_userinterview = j1.id;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                CalendarInterview_UserInterview calendarInterview_UserInterview = new CalendarInterview_UserInterview(reader.GetInt32("id"),
                                                                                                reader.GetInt32("id_calendarinterview"),
                                                                                                reader.GetInt32("id_userinterview"),
                                                                                                reader.GetString("name_User"),
                                                                                                reader.GetInt32("evaluate"),
                                                                                                reader["note_evaluate"] == DBNull.Value ? "" : reader.GetString("note_evaluate"));

                calendarInterview_UserInterviews.Add(calendarInterview_UserInterview);
            }

            conn.Close();


            return calendarInterview_UserInterviews;

        }

        public List<CalendarInterview_UserInterview> getAllCalendar_UserById_calendar(int id_calendar)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "SELECT calendarinterview_userinterview.*,j1.name_interview as 'name_User' FROM candidate_management.calendarinterview_userinterview inner join userinterview as j1 on calendarinterview_userinterview.id_userinterview = j1.id where id_calendarinterview = @id_calendar;";




            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@id_calendar", MySqlDbType.Int32).Value = id_calendar;




            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                CalendarInterview_UserInterview calendarInterview_UserInterview = new CalendarInterview_UserInterview(reader.GetInt32("id"),
                                                                                                reader.GetInt32("id_calendarinterview"),
                                                                                                reader.GetInt32("id_userinterview"),
                                                                                                reader.GetString("name_User"),
                                                                                                reader.GetInt32("evaluate"),
                                                                                                reader["note_evaluate"] == DBNull.Value ? "" : reader.GetString("note_evaluate"));

                calendarInterview_UserInterviews.Add(calendarInterview_UserInterview);
            }

            conn.Close();


            return calendarInterview_UserInterviews;

        }
        public void Delete(int id)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "delete from calendarinterview_userinterview where id_calendarinterview = @id_calendarinterview;";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@id_calendarinterview", MySqlDbType.Int32).Value = id;
            cmd.ExecuteNonQuery();

            conn.Close();

        }
    }
}
