using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Shoniz.Exception;


namespace Shoniz.Database_API
{
    public static class StoreProcdureManagement
    {
        /// <summary>
        /// The parameters dictionary is a static dictionary that will be filled while storeProcdure running. It will clear after
        /// SP running automaticly
        /// </summary>
        public static Dictionary<string, object> Parameters = new Dictionary<string, object>();

        /// <summary>
        /// Runs a store procedure.
        /// </summary>
        /// <typeparam name="T">The model which sp result will be converted to it</typeparam>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns>The list of T object type</returns>
        /// <exception>
        ///     <cref>SqlSpException ex</cref>
        /// </exception>
        public static List<T> RunSp<T>(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            List<T> list;
            var com = SqlGeneralMethods.MakeSqlCommand(connectionNameEnum, storeProcureName, Parameters);
            using (var result = com.ExecuteReader())
            {
                try
                {
                    list = SqlGeneralMethods.ToViewModel<T>(result);
                }
                catch (ShonizSqlSpException ex)
                {
                    Parameters.Clear(); 
                    throw;
                }
                finally
                {
                    ConnectionManager.ReleaseConnection(com.Connection);
                    Parameters.Clear();
                }
            }
            return list;
        }

        /// <summary>
        /// Runs the sp.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns>Scalar data of sp</returns>
        /// <exception>SqlSPException ex
        ///     <cref>SqlSpException ex</cref>
        /// </exception>
        public static object RunSp(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            object returnedObject = null;

            SqlCommand command = SqlGeneralMethods.MakeSqlCommand(connectionNameEnum, storeProcureName, Parameters);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                var schemaTable = dr.GetSchemaTable();
                if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                {
                    var sqlSpException = new ShonizSqlSpException();
                    sqlSpException.Add((int)dr["ErrorCode"], dr["ErrorMessage"].ToString());
                    while (dr.Read())
                    {
                        sqlSpException.Add((int)dr["ErrorCode"], dr["ErrorMessage"].ToString());
                    }

                    ConnectionManager.ReleaseConnection(command.Connection);
                    Parameters.Clear();
                    throw sqlSpException;
                }
                returnedObject = dr[0];
            }
            ConnectionManager.ReleaseConnection(command.Connection);
            Parameters.Clear();
            return returnedObject;
        }

        /// <summary>
        /// Runs the object list returned sp.
        /// </summary>
        /// <typeparam name="T">The model which sp result will be converted to it then they will be converted to object</typeparam>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns>
        /// The list of object. object is parent of T type
        /// </returns>    
        /// <exception>SqlSPException ex
        ///     <cref>SqlSPException</cref>
        /// </exception>
        public static List<object> RunObjectListReturnedSp<T>(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            List<object> list;
            try
            {
                var innerList = RunSp<T>(connectionNameEnum, storeProcureName);
                list = innerList.Cast<object>().ToList();
            }
            catch (ShonizSqlSpException ex)
            {
                Parameters.Clear();
                throw;
            }
            return list;
        } 
    }
}
