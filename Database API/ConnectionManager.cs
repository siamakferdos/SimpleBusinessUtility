using System.Data.SqlClient;
namespace Shoniz.Database_API
{
	public class ConnectionManager
	{
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum(An enum that has fields 
        /// which named by web.config connection string).</param>
        /// <returns>New Connection</returns>
        public static SqlConnection GetConnection(ConnectionNameEnum connectionNameEnum)
        {
            SqlConnection connection = null;
            connection = new SqlConnection(
                    System.Configuration.ConfigurationManager.
                    ConnectionStrings[connectionNameEnum.ToString()].ConnectionString);            
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Releases the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public static void ReleaseConnection(SqlConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open) return;
            connection.Close();
            connection.Dispose();
        }
	}
}