using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;


namespace MyProject.Server.Repository
{
    public class CandidateRepo
    {
        public void addCandidate(Candidate candidate)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();          
            string sql = "insert into candidate(idCandidate," +
                                                "roleCandidate," +
                                                "positionCandidate," +
                                                "nameCandidate," +
                                                "BirthDay," +
                                                "addressCandidate," +
                                                "numberphoneCandidate," +
                                                "emailCandidate," +
                                                "pathCVCandidate," +
                                                "originCandidate," +
                                                "contactingCandidate," +
                                                "appliedCandidate," +
                                                "statusCandidate," +
                                                "gradetestCandidate," +
                                                "noteCandidate," +
                                                "flagdltCandidate) values ( @idCandidate," +
                                                                            "@roleCandidate," +
                                                                            "@positionCandidate," +
                                                                            "@nameCandidate," +
                                                                            "@BirthDay," +
                                                                            "@addressCandidate," +
                                                                            "@numberphoneCandidate," +
                                                                            "@emailCandidate," +
                                                                            "@pathCVCandidate," +
                                                                            "@originCandidate," +
                                                                            "@contactingCandidate," +
                                                                            "@appliedCandidate," +
                                                                            "@statusCandidate," +
                                                                            "@gradetestCandidate," +
                                                                            "@noteCandidate," +
                                                                            "@flagdltCandidate);";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                // Create and set the parameters values 
                //cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = candidate.id;
                cmd.Parameters.Add("@idCandidate", MySqlDbType.VarChar).Value = candidate.strID;
                cmd.Parameters.Add("@roleCandidate", MySqlDbType.Int32).Value = candidate.Role;
                cmd.Parameters.Add("@positionCandidate", MySqlDbType.Int32).Value = candidate.Position;
                cmd.Parameters.Add("@nameCandidate", MySqlDbType.VarChar).Value = candidate.Name;
                cmd.Parameters.Add("@BirthDay", MySqlDbType.Date).Value = candidate.BirthDay;
                cmd.Parameters.Add("@addressCandidate", MySqlDbType.VarChar).Value = candidate.Address;
                cmd.Parameters.Add("@numberphoneCandidate", MySqlDbType.VarChar).Value = candidate.NumberPhone;
                cmd.Parameters.Add("@emailCandidate", MySqlDbType.VarChar).Value = candidate.Email;
                cmd.Parameters.Add("@pathCVCandidate", MySqlDbType.VarChar).Value = candidate.pathCV;
                cmd.Parameters.Add("@originCandidate", MySqlDbType.Int32).Value = candidate.Origin;
                cmd.Parameters.Add("@contactingCandidate", MySqlDbType.Int32).Value = candidate.Contacting;
                cmd.Parameters.Add("@appliedCandidate", MySqlDbType.Int32).Value = candidate.Applied;
                cmd.Parameters.Add("@statusCandidate", MySqlDbType.Int32).Value = candidate.Status;
                cmd.Parameters.Add("@gradetestCandidate", MySqlDbType.Int32).Value = candidate.gradeTest;
                cmd.Parameters.Add("@noteCandidate", MySqlDbType.VarChar).Value = candidate.Note;
                cmd.Parameters.Add("@flagdltCandidate", MySqlDbType.Int32).Value = candidate.FlagDlt;
                // Let's ask the db to execute the query
                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }

        public List<Candidate> candidates { get; set; } = new List<Candidate> { };

        public List<Candidate> GetCandidates()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT candidate.*, "+
                                                                            " j1.name_value as \"positionName\", " +
                                                                            " j2.name_value as \"roleName\", " +
                                                                            " j3.name_value as \"originName\", " +
                                                                            " j4.name_status as \"statusName\" FROM  candidate " +
                                                                                                                            " inner join valuesofcomb as j1 on candidate.positionCandidate = j1.id " +
                                                                                                                            " inner join valuesofcomb as j2 on candidate.roleCandidate = j2.id " +
                                                                                                                            " inner join valuesofcomb as j3 on candidate.originCandidate = j3.id " +
                                                                                                                            " inner join candidate_status as j4 on candidate.statusCandidate = j4.id_status order by candidate.statusCandidate ;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Candidate candidate = new Candidate(reader.GetInt32("id"),
                                                    reader.GetString("idCandidate"),
                                                    reader.GetString("nameCandidate"),
                                                    reader.GetInt32("roleCandidate"),
                                                    reader.GetInt32("positionCandidate"),
                                                    reader.GetDateTime("BirthDay"),
                                                    reader.GetString("addressCandidate"),
                                                    reader.GetInt32("numberphoneCandidate"),
                                                    reader.GetString("emailCandidate"),
                                                    reader.GetString("pathCVCandidate"),
                                                    reader.GetInt32("originCandidate"),
                                                    reader.GetInt32("contactingCandidate"),
                                                    reader.GetInt32("appliedCandidate"),
                                                    reader.GetInt32("statusCandidate"),
                                                    reader["gradetestCandidate"] == DBNull.Value ? "" : reader.GetString("gradetestCandidate"),
                                                    reader.GetString("noteCandidate"),
                                                    reader.GetInt32("flagdltCandidate"),
                                                    "");
                candidate.nameRole = reader.GetString("roleName");
                candidate.nameOrigin = reader.GetString("originName");
                candidate.namePosition = reader.GetString("positionName");
                candidate.nameStatus = reader.GetString("statusName");
                candidates.Add(candidate);
            }
            conn.Close();
            return candidates;

        }

        public List<Candidate> candidatesFilter { get; set; } = new List<Candidate> { };

        public List<Candidate> GetCandidatesFilter()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("select candidate.id,candidate.idCandidate, v1.name_value as \"roleName\",v2.name_value as \"positionName\",candidate.nameCandidate,candidate.BirthDay,candidate.addressCandidate,v3.name_value as \"originName\",candidate.appliedCandidate,candidate.pathCVCandidate  from candidate   inner  join valuesofcomb as v1 on candidate.roleCandidate = v1.id  inner join valuesofcomb as v2 on candidate.positionCandidate = v2.id inner join valuesofcomb as v3 on candidate.originCandidate= v3.id and candidate.statusCandidate = 1;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Candidate candidate = new Candidate(reader.GetInt32("id"),
                                                    reader.GetString("idCandidate"),
                                                    reader.GetString("roleName"),
                                                    reader.GetString("positionName"),
                                                    reader.GetString("nameCandidate"),
                                                    reader.GetDateTime("BirthDay"),
                                                    reader.GetString("addressCandidate"),
                                                    reader.GetString("originName"),
                                                    reader.GetInt32("appliedCandidate"),
                                                    reader.GetString("pathCVCandidate"));

                candidatesFilter.Add(candidate);

            }
            conn.Close();
            return candidatesFilter;

        }

        public List<Candidate> GetCandidatesBySTT(int stt)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("select candidate.*," +
                                                                            "j1.name_value as \"positionName\"," +
                                                                            "j2.name_value as \"roleName\"," +
                                                                            "j3.name_value as \"originName\"," +
                                                                            "j4.name_status as \"statusName\" from candidate inner join valuesofcomb as j1 on candidate.positionCandidate = j1.id " +
                                                                                                                             "inner join valuesofcomb as j2 on candidate.roleCandidate = j2.id " +
                                                                                                                             "inner join valuesofcomb as j3 on candidate.originCandidate = j3.id " +
                                                                                                                             "inner join candidate_status as j4 on candidate.statusCandidate = j4.id_status " +
                                                                                                                             "where candidate.statusCandidate = @stt ;", conn);
            mySqlCommand.Parameters.Add("stt", MySqlDbType.Int32).Value = stt;
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Candidate candidate = new Candidate(reader.GetInt32("id"),
                                                    reader.GetString("idCandidate"),
                                                    reader.GetInt32("roleCandidate"),
                                                    reader.GetInt32("positionCandidate"),
                                                    reader.GetString("nameCandidate"),
                                                    reader.GetDateTime("BirthDay"),
                                                    reader.GetString("addressCandidate"),
                                                    reader.GetInt32("numberphoneCandidate"),
                                                    reader.GetString("emailCandidate"),
                                                    reader.GetString("pathCVCandidate"),
                                                    reader.GetInt32("originCandidate"),
                                                    reader.GetInt32("contactingCandidate"),
                                                    reader.GetInt32("appliedCandidate"),
                                                    reader.GetInt32("statusCandidate"),
                                                    reader["gradetestCandidate"] == DBNull.Value ? "" : reader.GetString("gradetestCandidate"),
                                                    reader.GetString("noteCandidate"),
                                                    reader.GetInt32("flagdltCandidate"),
                                                    reader.GetString("positionName"),
                                                    reader.GetString("roleName"),
                                                    reader.GetString("originName"),
                                                    reader.GetString("statusName"));


                candidatesFilter.Add(candidate);

            }
            conn.Close();
            return candidatesFilter;

        }
        public Candidate GetCandidateByID(int id)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT candidate.*, "+
                                                                            " j1.name_value as \"positionName\", " +
                                                                            " j2.name_value as \"roleName\", " +
                                                                            " j3.name_value as \"originName\", " +
                                                                            " j4.name_status as \"statusName\" FROM  candidate " +
                                                                                                                            " inner join valuesofcomb as j1 on candidate.positionCandidate = j1.id " +
                                                                                                                            " inner join valuesofcomb as j2 on candidate.roleCandidate = j2.id " +
                                                                                                                            " inner join valuesofcomb as j3 on candidate.originCandidate = j3.id " +
                            
                                                                                                                            " inner join candidate_status as j4 on candidate.statusCandidate = j4.id_status WHERE candidate.id =" + id, conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            Candidate candidate = new Candidate();

            while (reader.Read())
            {
                candidate = new Candidate(reader.GetInt32("id"),
                                                    reader.GetString("idCandidate"),
                                                    reader.GetString("nameCandidate"),
                                                    reader.GetInt32("roleCandidate"),
                                                    reader.GetInt32("positionCandidate"),
                                                    reader.GetDateTime("BirthDay"),
                                                    reader.GetString("addressCandidate"),
                                                    reader.GetInt32("numberphoneCandidate"),
                                                    reader.GetString("emailCandidate"),
                                                    reader.GetString("pathCVCandidate"),
                                                    reader.GetInt32("originCandidate"),
                                                    reader.GetInt32("contactingCandidate"),
                                                    reader.GetInt32("appliedCandidate"),
                                                    reader.GetInt32("statusCandidate"),
                                                    reader["gradetestCandidate"] == DBNull.Value ? "" : reader.GetString("gradetestCandidate"),
                                                    reader.GetString("noteCandidate"),
                                                    reader.GetInt32("flagdltCandidate"),
                                                    "");
                candidate.nameRole = reader.GetString("roleName");
                candidate.nameOrigin = reader.GetString("originName");
                candidate.namePosition = reader.GetString("positionName");
                candidate.nameStatus = reader.GetString("statusName");
            }
            conn.Close();

            return candidate;

        }

        public bool update(Candidate candidate)
        {

            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            string sql = "UPDATE candidate SET  idCandidate = @idCandidate," +
                                                "roleCandidate = @roleCandidate," +
                                                "positionCandidate = @positionCandidate," +
                                                "nameCandidate = @nameCandidate," +
                                                "BirthDay = @BirthDay," +
                                                "addressCandidate = @addressCandidate," +
                                                "numberphoneCandidate = @numberphoneCandidate," +
                                                "emailCandidate = @emailCandidate," +
                                                "pathCVCandidate = @pathCVCandidate," +
                                                "originCandidate = @originCandidate," +
                                                "contactingCandidate = @contactingCandidate," +
                                                "appliedCandidate = @appliedCandidate," +
                                                "statusCandidate = @statusCandidate," +
                                                "gradetestCandidate = @gradetestCandidate," +
                                                "noteCandidate = @noteCandidate," +
                                                "flagdltCandidate = @flagdltCandidate WHERE id = @id";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {

                // Create and set the parameters values 
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = candidate.id;
                cmd.Parameters.Add("@idCandidate", MySqlDbType.VarChar).Value = candidate.strID;
                cmd.Parameters.Add("@roleCandidate", MySqlDbType.Int32).Value = candidate.Role;
                cmd.Parameters.Add("@positionCandidate", MySqlDbType.Int32).Value = candidate.Position;
                cmd.Parameters.Add("@nameCandidate", MySqlDbType.VarChar).Value = candidate.Name;
                cmd.Parameters.Add("@BirthDay", MySqlDbType.Date).Value = candidate.BirthDay;
                cmd.Parameters.Add("@addressCandidate", MySqlDbType.VarChar).Value = candidate.Address;
                cmd.Parameters.Add("@numberphoneCandidate", MySqlDbType.VarChar).Value = candidate.NumberPhone;
                cmd.Parameters.Add("@emailCandidate", MySqlDbType.VarChar).Value = candidate.Email;
                cmd.Parameters.Add("@pathCVCandidate", MySqlDbType.VarChar).Value = candidate.pathCV;
                cmd.Parameters.Add("@originCandidate", MySqlDbType.Int32).Value = candidate.Origin;
                cmd.Parameters.Add("@contactingCandidate", MySqlDbType.Int32).Value = candidate.Contacting;
                cmd.Parameters.Add("@appliedCandidate", MySqlDbType.Int32).Value = candidate.Applied;
                cmd.Parameters.Add("@statusCandidate", MySqlDbType.Int32).Value = candidate.Status;
                cmd.Parameters.Add("@gradetestCandidate", MySqlDbType.Int32).Value = candidate.gradeTest == "" ? null : candidate.gradeTest;
                cmd.Parameters.Add("@noteCandidate", MySqlDbType.VarChar).Value = candidate.Note;
                cmd.Parameters.Add("@flagdltCandidate", MySqlDbType.Int32).Value = candidate.FlagDlt;


                // Let's ask the db to execute the query
                cmd.ExecuteNonQuery();

            }

            conn.Close();

            return true;
        }
        public List<Candidate>  GetCandidatesEvaluate()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql = "SELECT candidate.*,j1.*,j2.name_value as 'positionName',j3.name_value as 'roleName',j4.name_value as 'originName',j5.name_status as 'statusName' " +
                        "FROM candidate_management.candidate " +

                        " inner join calendarinterview as j1 on candidate.id = j1.id_candidate " +

                        "inner join valuesofcomb as j2 on candidate.positionCandidate = j2.id " +
                        " inner join valuesofcomb as j3 on candidate.roleCandidate = j3.id " +
                        " inner join valuesofcomb as j4 on candidate.originCandidate = j4.id " +
                        " inner join candidate_status as j5 on candidate.statusCandidate = j5.id_status,calendarinterview " +

                         "where candidate.statusCandidate in (6,8) and j1.room_interview in (1,2) and calendarinterview.name_interview in ('PV V1', 'PV V2') and j1.time_interview < now() group by candidate.id;";



            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            //SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));
            while (reader.Read())
            {
                var candidate = new Candidate();

                candidate.id = reader.GetInt32("id");
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Position = reader.GetInt32("positionCandidate");
                candidate.Role = reader.GetInt32("roleCandidate");

                candidate.nameRole = reader.GetString("roleName");
                candidate.namePosition = reader.GetString("positionName");
                candidate.Name = reader.GetString("nameCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.Email = reader.GetString("emailCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.timeInterview = reader.GetDateTime("time_interview");
                candidate.Address = reader.GetString("addressCandidate");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.nameStatus = reader.GetString("statusName");
                candidate.Status = reader.GetInt32("statusCandidate");
                candidate.pathCV = reader.GetString("pathCVCandidate");
                candidate.gradeTest = reader.GetString("gradetestCandidate");
                candidate.addressInterview = reader.GetString("addressCandidate");
                candidate.Origin = reader.GetInt32("originCandidate");
                //lịch
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.name_interview = reader.GetString("name_interview");
                candidate.room_interview = reader.GetInt32("room_interview");

                candidates.Add(candidate);
                
            }
            conn.Close();
            return candidates;
        }
        public List<Candidate> GetCandidatesEvaluated()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql ="SELECT candidate.*,j1.*,j2.name_value as 'positionName',j3.name_value as 'roleName',j4.name_value as 'originName',j5.name_status as 'statusName' " +
                        " FROM candidate_management.candidate " + 

                        " inner join calendarinterview as j1 on candidate.id = j1.id_candidate " +

                        " inner join valuesofcomb as j2 on candidate.positionCandidate = j2.id " +
                        " inner join valuesofcomb as j3 on candidate.roleCandidate = j3.id " +
                        " inner join valuesofcomb as j4 on candidate.originCandidate = j4.id " +
                        " inner join candidate_status as j5 on candidate.statusCandidate = j5.id_status,calendarinterview " +


                        " where candidate.statusCandidate in (2,5, 7, 9) and calendarinterview.name_interview in ('Mời Test', 'PV V1', 'PV V2') group by candidate.id; ";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            //SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));

            while (reader.Read())
            {
                var candidate = new Candidate();

                candidate.id = reader.GetInt32("id");
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Position = reader.GetInt32("positionCandidate");
                candidate.Role = reader.GetInt32("roleCandidate");

                candidate.nameRole = reader.GetString("roleName");
                candidate.namePosition = reader.GetString("positionName");
                candidate.Name = reader.GetString("nameCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.Email = reader.GetString("emailCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.timeInterview = reader.GetDateTime("time_interview");
                candidate.addressInterview = reader.GetString("address_interview");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.nameStatus = reader.GetString("statusName");
                candidate.Status = reader.GetInt32("statusCandidate");
                //lịch
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.name_interview = reader.GetString("name_interview");

                candidate.room_interview = reader["room_interview"] == DBNull.Value ? 0 : reader.GetInt32("room_interview");
                
                candidates.Add(candidate);

            }
            conn.Close();
            return candidates;
        }

        public List<Candidate> GetEvaluateOfCandidate(int id_candidate)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql = "select candidate.*, calendarinterview.*,calendarinterview_userinterview.evaluate,calendarinterview_userinterview.note_evaluate,calendarinterview_userinterview.id_userinterview,j2.name_interview as 'userinterviewName' ,j1.name_status as 'statusName' " +
                "from  candidate inner join candidate_status as j1 on candidate.statusCandidate = j1.id_status,  calendarinterview  ,  " +
                "  calendarinterview_userinterview inner join userinterview as j2 on calendarinterview_userinterview.id_userinterview = j2.id " +
                "where candidate.id = calendarinterview.id_candidate and calendarinterview.id_interview = calendarinterview_userinterview.id_calendarinterview and candidate.id = @id_candidate ;";
                        

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("id_candidate", MySqlDbType.Int32).Value = id_candidate;

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var candidate = new Candidate();

                candidate.id = reader.GetInt32("id");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Role = reader.GetInt32("roleCandidate");
                candidate.Position = reader.GetInt32("positionCandidate");
                
                candidate.Name = reader.GetString("nameCandidate");
                candidate.BirthDay = reader.GetDateTime("BirthDay");
                candidate.Address = reader.GetString("addressCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.nameRole = reader.GetString("emailCandidate");
                candidate.pathCV = reader.GetString("pathCVCandidate");
                candidate.Origin = reader.GetInt32("originCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.Applied = reader.GetInt32("appliedCandidate");
                candidate.Status = reader.GetInt32("statusCandidate");
                candidate.gradeTest = reader.GetString("gradetestCandidate");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.FlagDlt = reader.GetInt32("flagdltCandidate");
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.name_interview = reader.GetString("name_interview");
                candidate.timeInterview = reader.GetDateTime("time_interview");
                candidate.addressInterview = reader.GetString("address_interview");
                candidate.room_interview = reader["room_interview"] == DBNull.Value ? 0 : reader.GetInt32("room_interview");
                candidate.evaluate = reader.GetInt32("evaluate");
                candidate.note_evaluate = reader["note_evaluate"] == DBNull.Value ? "" : reader.GetString("note_evaluate");
                candidate.id_userinterview = reader.GetInt32("id_userinterview");
                candidate.userinterviewName = reader.GetString("userinterviewName");
                candidate.nameStatus = reader.GetString("statusName");

                candidates.Add(candidate);

            }
            conn.Close();
            return candidates;
        }

        public Candidate GetGradeTestOfCandidate(int id_candidate)
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql = "SELECT candidate.*,j1.*,j2.name_value as 'positionName',j3.name_value as 'roleName',j4.name_value as 'originName',j5.name_status as 'statusName' " +
                            "FROM candidate_management.candidate " +
                            " inner join calendarinterview as j1 on candidate.id = j1.id_candidate " +
                            " inner join valuesofcomb as j2 on candidate.positionCandidate = j2.id " +
                            " inner join valuesofcomb as j3 on candidate.roleCandidate = j3.id " +
                            " inner join valuesofcomb as j4 on candidate.originCandidate = j4.id " +
                            " inner join candidate_status as j5 on candidate.statusCandidate = j5.id_status,calendarinterview " +
                            " Where calendarinterview.name_interview = 'Mời Test' " +
                            " and candidate.id = @id_candidate "+
                            " group by candidate.id;";


            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("id_candidate", MySqlDbType.Int32).Value = id_candidate;

            MySqlDataReader reader = cmd.ExecuteReader();
            var candidate = new Candidate();


            while (reader.Read())
            {
               

                candidate.id = reader.GetInt32("id");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Role = reader.GetInt32("roleCandidate");
                candidate.Position = reader.GetInt32("positionCandidate");

                candidate.Name = reader.GetString("nameCandidate");
                candidate.BirthDay = reader.GetDateTime("BirthDay");
                candidate.nameRole = reader.GetString("addressCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.nameRole = reader.GetString("emailCandidate");
                candidate.pathCV = reader.GetString("pathCVCandidate");
                candidate.Origin = reader.GetInt32("originCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.Applied = reader.GetInt32("appliedCandidate");
                candidate.Status = reader.GetInt32("statusCandidate");
                candidate.gradeTest = reader.GetString("gradetestCandidate");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.FlagDlt = reader.GetInt32("flagdltCandidate");


                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.name_interview = reader.GetString("name_interview");

                candidate.timeInterview = reader.GetDateTime("time_interview");

                candidate.addressInterview = reader.GetString("address_interview");

                candidate.room_interview = reader["room_interview"] == DBNull.Value ? 0 : reader.GetInt32("room_interview");

                candidate.nameRole = reader.GetString("roleName");
                candidate.nameOrigin = reader.GetString("originName");
                candidate.namePosition = reader.GetString("positionName");


                candidate.nameStatus = reader.GetString("statusName");

                candidates.Add(candidate);

            }
            conn.Close();
            return candidate;
        }

        public List<Candidate>  GetCandidateSendOffer()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql = "SELECT candidate.*,j2.name_value as 'positionName',j3.name_value as 'roleName',j4.name_value as 'originName',j5.name_status as 'statusName'  " +
                         " FROM candidate_management.candidate " +
                         " inner join valuesofcomb as j2 on candidate.positionCandidate = j2.id " +
                         " inner join valuesofcomb as j3 on candidate.roleCandidate = j3.id " +
                         " inner join valuesofcomb as j4 on candidate.originCandidate = j4.id " +
                         " inner join candidate_status as j5 on candidate.statusCandidate = j5.id_status,calendarinterview " +
                         " where candidate.statusCandidate = 9 group by candidate.id;";


            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            


            while (reader.Read())
            {
                var candidate = new Candidate();

                candidate.id = reader.GetInt32("id");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Role = reader.GetInt32("roleCandidate");
                candidate.Position = reader.GetInt32("positionCandidate");

                candidate.Name = reader.GetString("nameCandidate");
                candidate.BirthDay = reader.GetDateTime("BirthDay");
                candidate.nameRole = reader.GetString("addressCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.Email = reader.GetString("emailCandidate");
                candidate.nameRole = reader.GetString("emailCandidate");
                candidate.pathCV = reader.GetString("pathCVCandidate");
                candidate.Origin = reader.GetInt32("originCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.Applied = reader.GetInt32("appliedCandidate");
                candidate.Status = reader.GetInt32("statusCandidate");
                candidate.gradeTest = reader.GetString("gradetestCandidate");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.FlagDlt = reader.GetInt32("flagdltCandidate");

                candidate.nameRole = reader.GetString("roleName");
                candidate.nameOrigin = reader.GetString("originName");
                candidate.namePosition = reader.GetString("positionName");


                candidate.nameStatus = reader.GetString("statusName");

                candidates.Add(candidate);

            }
            conn.Close();
            return candidates;
        }


        public List<Candidate> GetCandidatesAddTTInterview()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();
            string sql = "SELECT candidate.*,calendarinterview.*,j1.name_value as 'positionName',j2.name_value as 'roleName',j3.name_value as 'originName',j4.name_status as 'statusName' " +
                        "FROM candidate	" +
                        "inner join valuesofcomb as j1 on candidate.positionCandidate = j1.id " +
                        " inner join valuesofcomb as j2 on candidate.roleCandidate = j2.id " +
                        " inner join valuesofcomb as j3 on candidate.originCandidate = j3.id " +
                        " inner join candidate_status as j4 on candidate.statusCandidate = j4.id_status,calendarinterview " +
                        "where candidate.id = calendarinterview.id_candidate and candidate.statusCandidate in (6, 8) and calendarinterview.name_interview in ('PV V1', 'PV V2') and calendarinterview.room_interview is null; ";



            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var candidate = new Candidate();

                candidate.id = reader.GetInt32("id");
                candidate.id_calendar = reader.GetInt32("id_interview");
                candidate.strID = reader.GetString("idCandidate");

                candidate.Position = reader.GetInt32("positionCandidate");
                candidate.Role = reader.GetInt32("roleCandidate");

                candidate.nameRole = reader.GetString("roleName");
                candidate.namePosition = reader.GetString("positionName");
                candidate.Name = reader.GetString("nameCandidate");
                candidate.NumberPhone = reader.GetInt32("numberphoneCandidate");
                candidate.Email = reader.GetString("emailCandidate");
                candidate.Contacting = reader.GetInt32("contactingCandidate");
                candidate.timeInterview = reader.GetDateTime("time_interview");
                candidate.addressInterview = reader.GetString("address_interview");
                candidate.Note = reader.GetString("noteCandidate");
                candidate.nameStatus = reader.GetString("statusName");
                candidate.nameRole = reader.GetString("roleName");
                candidates.Add(candidate);

            }
            conn.Close();
            return candidates;
        }

        public void DeleteCandidate(int id_candidate)
        {
            MySqlConnection conn = AppSettings.GetConnection();
            conn.Open();
            string sql = "DELETE FROM candidate_management.candidate WHERE id = @id ; ";

            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            mySqlCommand.Parameters.Add("@id",MySqlDbType.Int32).Value = id_candidate;

            mySqlCommand.ExecuteNonQuery(); 
            conn.Close();
        }

    }
}
