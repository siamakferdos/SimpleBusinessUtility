using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Shoniz.Common.Data.DataConvertor;

namespace Shoniz.Common.Data.SqlServer
{
    internal class GeneralMethods
    {
        internal int RecordCount = 0;

        /// <summary>
        /// Read and fill sql data reader into T type object
        /// </summary>
        /// <typeparam name="T">The type that sq data reader records will be converted to it</typeparam>
        /// <param name="sqlDataReader">The dr object.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns>List of T type</returns>
        internal List<T> ToViewModel<T>(SqlDataReader sqlDataReader, int pageIndex = 0, int pageLength = 0)
        {
            if (pageIndex <= 0)
                return new DataReaderToList().Convert<T>(sqlDataReader);
            return new DataReaderToList().Convert<T>(sqlDataReader).GetRange((pageIndex - 1) * pageLength, pageLength);
        }

        internal List<T> ToViewModel<T>(SqlDataReader sqlDataReader, int recordCount, int pageNumber,
            out int allRecordCount)
        {
            var list = new DataReaderToList().Convert<T>(sqlDataReader);
            allRecordCount = list.Count();
            if (allRecordCount > 0)
                return list.Skip((pageNumber - 1) * recordCount).Take(recordCount).ToList();
            return new List<T>();
        }


        /// <summary>
        /// To the view model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public List<T> ToViewModel<T>(IEnumerable<DataRow> enumerable)
        {
            var dt = new DataTable();
            foreach (DataRow row in enumerable)
            {
                dt.ImportRow(row);
            }
            return new DataTableToList().Convert<T>(dt);//.GetRange((pageIndex - 1) * pageLength, pageLength);
        }


        /// <summary>
        /// Makes the SQL command.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="connectionTimeOut"></param>
        /// <returns></returns>
        internal SqlCommand MakeSqlCommand(ConnectionNameEnum connectionNameEnum, string storeProcureName, List<SqlParameter> parameters, int connectionTimeOut)
        {
            var sqlCommand = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = storeProcureName };

            sqlCommand.Parameters.AddRange(parameters.ToArray());
            //foreach (var r in parameters)
            //{
            //    sqlCommand.Parameters.Add(new SqlParameter(r.Key, r.Value));
            //}
            sqlCommand.Connection = new ConnectionManager().GetConnection(connectionNameEnum);
            sqlCommand.CommandTimeout = connectionTimeOut;
            return sqlCommand;
        }



        internal SqlCommand MakeSqlCommand(string connectionNameEnum, string storeProcureName, List<SqlParameter> parameters, int connectionTimeOut)
        {
            var sqlCommand = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = storeProcureName };

            sqlCommand.Parameters.AddRange(parameters.ToArray());
            //foreach (var r in parameters)
            //{
            //    sqlCommand.Parameters.Add(new SqlParameter(r.Key, r.Value));
            //}
            sqlCommand.Connection = new ConnectionManager().GetConnection(connectionNameEnum);
            sqlCommand.CommandTimeout = connectionTimeOut;
            return sqlCommand;
        }

        /// <summary>
        /// Makes the SQL command.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="query"></param>
        /// <param name="connectionTimeOut"></param>
        /// <returns></returns>
        internal static SqlCommand MakeQuerySqlCommand(ConnectionNameEnum connectionNameEnum, string query, int connectionTimeOut, List<SqlParameter> parameters = null)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = query,
                Connection = new ConnectionManager().GetConnection(connectionNameEnum)
            };
            if (parameters != null)
                sqlCommand.Parameters.AddRange(parameters.ToArray());

            sqlCommand.CommandTimeout = connectionTimeOut;
            return sqlCommand;
        }

        /// <summary>
        /// Makes the SQL command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="query">The query.</param>
        /// <param name="connectionTimeOut"></param>
        /// <returns></returns>
        internal static SqlCommand MakeQuerySqlCommand(string connectionString, string query, int connectionTimeOut, List<SqlParameter> parameters = null)
        {
            var sqlCommand = new SqlCommand
            {
                CommandText = query,
                Connection = new ConnectionManager().GetConnection(connectionString)
            };

            if (parameters != null)
                sqlCommand.Parameters.AddRange(parameters.ToArray());

            sqlCommand.CommandTimeout = connectionTimeOut;
            return sqlCommand;
        }
    }
}