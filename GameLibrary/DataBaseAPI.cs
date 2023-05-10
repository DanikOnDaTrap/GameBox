using System.Data.SqlClient;

namespace GameLibrary
{
    class DataBaseAPI
    {
        SqlConnection cn;

        public DataBaseAPI(string dbName, string serverName)
        {
            SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder();
            connect.InitialCatalog = $"{dbName}"; // Наименование БД
            connect.DataSource = $@"{serverName}"; // Сервер
            connect.ConnectTimeout = 5;
            connect.IntegratedSecurity = true;
            cn = new SqlConnection();
            cn.ConnectionString = connect.ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return 
                cn;
        }

        public void OpenConnection()
        {
            cn.Open();
        }

        public void CloseConnection()
        {
            cn.Close();
        }
    }
}