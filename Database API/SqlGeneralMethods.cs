using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shoniz.Exception;


namespace Shoniz.Database_API
{
    public static class SqlGeneralMethods
    {
        /// <summary>
        /// Read and fill sql data reader into T type object
        /// </summary>
        /// <typeparam name="T">The type that sq data reader records will be converted to it</typeparam>
        /// <param name="sqlDataReader">The sqlDataReader object.</param>
        /// <returns>List of T type</returns>
        public static List<T> ToViewModel<T>(SqlDataReader sqlDataReader)
        {
            var list = new List<T>();
            while (sqlDataReader.Read())
            {
                var schemaTable = sqlDataReader.GetSchemaTable();
                if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                {
                    var sqlSpException = new ShonizSqlSpException();
                    sqlSpException.Add((int)sqlDataReader["ErrorCode"], sqlDataReader["ErrorMessage"].ToString());
                    while (sqlDataReader.Read())
                    {
                        sqlSpException.Add((int)sqlDataReader["ErrorCode"], sqlDataReader["ErrorMessage"].ToString());
                    }
                    throw sqlSpException;
                }
                var o = (T)Activator.CreateInstance(typeof(T));
                foreach (var prop in o.GetType().GetProperties())
                {
                    try
                    {
                        prop.SetValue(o, sqlDataReader[prop.Name], null);
                    }
                    catch { }
                }
                list.Add(o);
            }
            return list;
        }

        /// <summary>
        /// Makes the SQL command.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static SqlCommand MakeSqlCommand(ConnectionNameEnum connectionNameEnum, string storeProcureName, Dictionary<string, object> parameters)
        {
            var sqlCommand = new SqlCommand {CommandType = CommandType.StoredProcedure, CommandText = storeProcureName};

            foreach (var r in parameters)
            {
                sqlCommand.Parameters.Add(new SqlParameter(r.Key, r.Value));
            }
            sqlCommand.Connection = ConnectionManager.GetConnection(connectionNameEnum);
            return sqlCommand;
        }
    }
}