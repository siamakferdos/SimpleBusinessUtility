using System.Data.SqlClient;

namespace Shoniz.Common.Data.SqlServer
{
	internal class ConnectionManager
	{
        internal SqlConnection GetConnection(ConnectionNameEnum connectionNameEnum)
        {
            var connection = new SqlConnection(
                System.Configuration.ConfigurationManager.
                    ConnectionStrings[connectionNameEnum.ToString()].ConnectionString);            
            connection.Open();
            return connection;
        }

        internal SqlConnection GetConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Releases the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        internal static void ReleaseConnection(SqlConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Closed) return;
            try
            {                
                connection.Close();
                connection.Dispose();
            }
            catch { }
        }
	}
}