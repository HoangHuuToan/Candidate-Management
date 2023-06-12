using MyProject.Shared.Entities;
using MyProject.Shared.Path;
using MySql.Data.MySqlClient;

namespace MyProject.Server.Repository
{
    public class ValuesComb
    {
        public List<ValuesOfComb> ValuesOfComb { get; set; } = new List<ValuesOfComb> { };

        public List<ValuesOfComb> getValuesComb()
        {
            MySqlConnection conn = AppSettings.GetConnection();

            conn.Open();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM  valuesofcomb",conn);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                ValuesOfComb valueOfComb = new ValuesOfComb(reader.GetInt32("id"),reader.GetInt32("valueOfComb"),reader.GetString("name_value"));
                ValuesOfComb.Add(valueOfComb);
            }
            conn.Close();
            return ValuesOfComb;
        }
    }
}
