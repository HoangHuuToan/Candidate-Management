using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;

namespace MyProject.Server.Repository
{
    public class TemplateMailRepo
    {
        public TemplateMail templateMail { get; set; } = new TemplateMail { };

        public List<TemplateMail> templateMails { get; set; } = new List<TemplateMail>();

        public TemplateMail getTemplateMail(int id) 
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM  templatemail WHERE id = @id ;", conn);
            mySqlCommand.Parameters.Add("@id",MySqlDbType.Int32).Value = id;

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                templateMail = new TemplateMail(reader.GetInt32("id"),
                                                    reader.GetString("name_mail"),
                                                    reader.GetString("title_mail"),
                                                    reader.GetString("content_mail"));
            }
            conn.Close();
            return templateMail;
        }
        public List<TemplateMail> getAllTemplateMail()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM  templatemail ;", conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                TemplateMail templateMail = new TemplateMail(reader.GetInt32("id"),
                                                    reader.GetString("name_mail"),
                                                    reader.GetString("title_mail"),
                                                    reader.GetString("content_mail"));

                templateMails.Add(templateMail);
            }
            conn.Close();
            return templateMails;
        }
    }
}
