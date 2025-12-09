using System.Configuration;
using MySql.Data.MySqlClient;

namespace ECommerce.Data
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ECommerceDB"]?.ConnectionString
                    ?? "Server=localhost;Database=ecommerce_db;User Id=root;Password=;";
            }
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
