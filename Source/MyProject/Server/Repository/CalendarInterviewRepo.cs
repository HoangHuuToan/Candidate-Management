using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Transactions;

namespace MyProject.Server.Repository
{
    public class CalendarInterviewRepo
    {
        public List<CalendarInterview> calendarInterviews = new List<CalendarInterview>();
        public void addCalendarInterview(CalendarInterview calendarInterview)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "insert into calendarinterview(name_interview," +
                                                "id_candidate," +
                                                "time_interview," +
                                                "address_interview" +
                                                ") values (" +
                                                            "@name_interview," +
                                                            "@id_candidate," +
                                                            "@time_interview," +
                                                            "@address_interview" +
                                                            ");";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                // Create and set the parameters values 
                //cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = candidate.id;
                cmd.Parameters.Add("@name_interview", MySqlDbType.VarChar).Value = calendarInterview.nameInterview;
                cmd.Parameters.Add("@id_candidate", MySqlDbType.Int32).Value = calendarInterview.idCandidate;
                cmd.Parameters.Add("@time_interview", MySqlDbType.DateTime).Value = calendarInterview.timeInterview;
                cmd.Parameters.Add("@address_interview", MySqlDbType.VarChar).Value = calendarInterview.addressInterview;

                // Let's ask the db to execute the query
                cmd.ExecuteNonQuery();


            }

            conn.Close();


        }

        public enum Status
        {
            [Display(Name = "Mời Test")]
            invatation_test = 1,
            [Display(Name = "PV V1")]
            interview_round_1,
            [Display(Name = "PV V2")]
            interview_round_2,
            [Display(Name = "Gửi Offer")]
            send_offer
        }

        public static string GetEnumDisplayName(Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }

        public List<CalendarInterview> GetCalendarStt(int stt)
        {
            string name_interview = GetEnumDisplayName((Status)stt);

            //if (stt == 1)
            //{
            //    name_interview = Status.invatation_test;
            //}
            //else if (stt == 2)       
            //{
            //    name_interview = "PV V1";
            //}
            //else if (stt == 3)
            //{
            //    name_interview = "PV V2";
            //}
            //else if (stt == 4)
            //{
            //    name_interview = "Gửi Offer";
            //}

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM calendarinterview WHERE name_interview = @name_interview ; ", conn);

            mySqlCommand.Parameters.Add("name_interview", MySqlDbType.String).Value = name_interview;

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CalendarInterview calendarInterview = new CalendarInterview(reader.GetInt32("id_interview"),
                                                                    reader.GetString("name_interview"),
                                                                    reader.GetInt32("id_candidate"),
                                                                    reader.GetDateTime("time_interview"),
                                                                    reader.GetString("address_interview"));

                calendarInterviews.Add(calendarInterview);
            }
            conn.Close();
            return calendarInterviews;

        }

        public List<CalendarInterview> GetAllCalendar()
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM calendarinterview;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CalendarInterview calendarInterview = new CalendarInterview(reader.GetInt32("id_interview"),
                                                                            reader.GetString("name_interview"),
                                                                            reader.GetInt32("id_candidate"),
                                                                            reader.GetDateTime("time_interview"),
                                                                            reader.GetString("address_interview"));

                calendarInterviews.Add(calendarInterview);
            }
            conn.Close();
            return calendarInterviews;

        }

        public bool UpdateCalendar(CalendarInterview calendarInterview)
        {

            MySqlConnection conn = AppSettings.GetConnection();
            conn.Open();

            string sql = "UPDATE calendarinterview SET  name_interview = @name_interview," +
                                                "id_candidate = @id_candidate," +
                                                "time_interview = @time_interview," +
                                                "address_interview = @address_interview, " +
                                                "room_interview = @room_interview WHERE id_candidate = @id_candidate AND name_interview = @name_interview";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                // Create and set the parameters values 
                //cmd.Parameters.Add("@id_interview", MySqlDbType.Int32).Value = calendarInterview.id;

                cmd.Parameters.Add("@name_interview", MySqlDbType.VarChar).Value = calendarInterview.nameInterview;
                cmd.Parameters.Add("@id_candidate", MySqlDbType.Int32).Value = calendarInterview.idCandidate;
                cmd.Parameters.Add("@time_interview", MySqlDbType.DateTime).Value = calendarInterview.timeInterview;
                cmd.Parameters.Add("@address_interview", MySqlDbType.VarChar).Value = calendarInterview.addressInterview;
                cmd.Parameters.Add("@room_interview", MySqlDbType.Int32).Value = calendarInterview.roominterview;


                // Let's ask the db to execute the query
                cmd.ExecuteNonQuery();

            }

            conn.Close();

            return true;

        }
        public List<CalendarInterview> GetAllCalendarHasRoom()
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT calendarinterview.*,candidate.nameCandidate FROM calendarinterview,candidate where room_interview in (1,2) and calendarinterview.id_candidate = candidate.id and candidate.statusCandidate in (6,8);", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CalendarInterview calendarInterview = new CalendarInterview(reader.GetInt32("id_interview"),
                                                                            reader.GetString("name_interview"),
                                                                            reader.GetInt32("id_candidate"),
                                                                            reader.GetDateTime("time_interview"),
                                                                            reader.GetString("address_interview"),
                                                                            reader.GetString("nameCandidate"),
                                                                            reader.GetInt32("room_interview"));

                calendarInterviews.Add(calendarInterview);
            }
            conn.Close();
            return calendarInterviews;

        }

        public bool DeleteCalendar(int id)
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "update  calendarinterview set room_interview = null where id_interview =@id_interview;";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                cmd.Parameters.Add("@id_interview", MySqlDbType.Int32).Value = id;
                
                cmd.ExecuteNonQuery();

            }

            conn.Close();

            return true;

        }


    }
}
