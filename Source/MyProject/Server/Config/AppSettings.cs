using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MyProject.Shared.Path
{
    public static class AppSettings
    {
        private static IConfiguration _IConfiguration;

        public static void AppSettingsConfig(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public static MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(_IConfiguration.GetConnectionString("ConnectTo_DB"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetJwtSecret()
        {
            try
            {
                return _IConfiguration.GetValue<string>("Jwt:Key");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetJwtIssuer()
        {
            try
            {
                return _IConfiguration.GetValue<string>("Jwt:Issuer");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}