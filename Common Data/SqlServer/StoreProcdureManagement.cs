using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Shoniz.Common.Core;

namespace Shoniz.Common.Data.SqlServer
{
    public class StoreProcdureManagement
    {
        /// <summary>
        /// The parameters dictionary is a static dictionary that will be filled while storeProcdure running. It will clear after
        /// SP running automaticly
        /// </summary>
        public List<SqlParameter> Parameters = new List<SqlParameter>();

        public int ConnectionTimeout = 30;
        public int CurrentListLength = 0;
        private string _ConnectionString = "";

        /// <summary>
        /// Runs a store procedure.
        /// </summary>
        /// <typeparam name="T">The model which sp result will be converted to it</typeparam>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns>The list of T object type</returns>
        /// <exception>
        ///     <cref>SqlSpException ex</cref>
        /// </exception>
        public List<T> RunSp<T>(ConnectionNameEnum connectionNameEnum, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            List<T> list;
            SqlCommand com;
            if (connectionNameEnum == ConnectionNameEnum.DirectConnectionString)
                com = new GeneralMethods().MakeSqlCommand(_ConnectionString, storeProcureName, param, ConnectionTimeout);
            else
                com = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);

            try
            {
                using (var result = com.ExecuteReader())
                {

                    var schemaTable = result.GetSchemaTable();
                    if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                    {
                        var sqlSpException = new DataException();
                        //sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        while (result.Read())
                        {
                            sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        }
                        throw sqlSpException;
                    }

                    if (typeof(T).IsValueType || "".GetType() == typeof(T))
                    {
                        list = new List<T>();
                        Parallel.ForEach(SimpleParallelDataReader<T>(result), (data) =>
                        {
                            list.Add(data);
                        });
                        return list;
                    }

                    var gm = new GeneralMethods();
                    list = gm.ToViewModel<T>(result, pageIndex, pageLength);
                    CurrentListLength = gm.RecordCount;
                }
            }
            catch (DataException)
            {
                Parameters.Clear();
                throw;
            }
            catch (Exception ex)
            {
                var exp = new DataException();
                exp.Data.Add("Error", ex);
                throw exp;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(com.Connection);
            }

            return list;
        }

        public List<dynamic> RunSpReturnDynamic(ConnectionNameEnum connectionNameEnum, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            List<dynamic> list = new List<dynamic>();
            SqlCommand com;
            com = connectionNameEnum == ConnectionNameEnum.DirectConnectionString ?
                new GeneralMethods().MakeSqlCommand(_ConnectionString, storeProcureName, param, ConnectionTimeout) :
                new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);

            try
            {
                using (var result = com.ExecuteReader())
                {

                    var schemaTable = result.GetSchemaTable();
                    if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                    {
                        var sqlSpException = new DataException();
                        //sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        while (result.Read())
                        {
                            sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        }
                        throw sqlSpException;
                    }


                    Parallel.ForEach(DynamicParallelDataReader(result, schemaTable),
                        (data) =>
                        {
                            list.Add(data);
                        });
                }
            }
            catch (DataException)
            {
                Parameters.Clear();
                throw;
            }
            catch (Exception ex)
            {
                var exp = new DataException();
                exp.Data.Add("Error", ex);
                throw exp;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(com.Connection);
            }

            return list;
        }

        /// <summary>
        /// Runs the sp with string connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageLength">Length of the page.</param>
        /// <returns></returns>

        public List<T> RunSp<T>(string connectionString, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            _ConnectionString = connectionString;
            return RunSp<T>(ConnectionNameEnum.DirectConnectionString, storeProcureName, pageIndex, pageLength);
        }

        public List<dynamic> RunSpReturnDynamic(string connectionString, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            _ConnectionString = connectionString;
            return RunSpReturnDynamic(ConnectionNameEnum.DirectConnectionString, storeProcureName, pageIndex, pageLength);
        }

        public async Task<List<T>> RunSpAsync<T>(ConnectionNameEnum connectionNameEnum, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            List<T> list;
            SqlCommand com;
            if (connectionNameEnum == ConnectionNameEnum.DirectConnectionString)
                com = new GeneralMethods().MakeSqlCommand(_ConnectionString, storeProcureName, param, ConnectionTimeout);
            else
                com = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);
            using (var result = await com.ExecuteReaderAsync())
            {
                try
                {
                    var schemaTable = result.GetSchemaTable();
                    if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                    {
                        var sqlSpException = new DataException();
                        //sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        while (result.Read())
                        {
                            sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        }
                        throw sqlSpException;
                    }

                    if (typeof(T).IsValueType || "".GetType() == typeof(T))
                    {
                        list = new List<T>();
                        Parallel.ForEach(SimpleParallelDataReader<T>(result), (data) =>
                        {
                            list.Add(data);
                        });
                        return list;
                    }

                    var gm = new GeneralMethods();
                    list = gm.ToViewModel<T>(result, pageIndex, pageLength);
                    CurrentListLength = gm.RecordCount;
                }
                catch (DataException)
                {
                    Parameters.Clear();
                    throw;
                }
                finally
                {
                    ConnectionManager.ReleaseConnection(com.Connection);
                }
            }
            return list;
        }
        public async Task<List<T>> RunSpAsync<T>(string connectionString, string storeProcureName, int pageIndex = 0, int pageLength = 0)
        {
            _ConnectionString = connectionString;
            return await RunSpAsync<T>(ConnectionNameEnum.DirectConnectionString, storeProcureName, pageIndex, pageLength);
        }
        /// <summary>
        /// Runs the sp with string assembly name to convert.It is used for dynamic porpose.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns></returns>
        public IList RunSp(string assemblyName, string typeName, ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            MethodInfo method = typeof(StoreProcdureManagement).GetMethods().First(m => m.Name == "RunSp" && m.GetGenericArguments().Length == 1);
            var typeArgs = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
            MethodInfo generic = method.MakeGenericMethod(typeArgs);
            return (IList)generic.Invoke(this, new[] { (object)connectionNameEnum, (object)storeProcureName, (object)0, (object)0 });
        }


        public List<T> RunQuery<T>(string connectionString, string query)
        {
            _ConnectionString = connectionString;
            return RunQuery<T>(ConnectionNameEnum.DirectConnectionString, query);
        }

        public List<T> RunQuery<T>(ConnectionNameEnum connectionNameEnum, string query)
        {
            List<T> list;
            SqlCommand com;
            if (connectionNameEnum == ConnectionNameEnum.DirectConnectionString)
                com = GeneralMethods.MakeQuerySqlCommand(_ConnectionString, query, ConnectionTimeout, Parameters);
            else
                com = GeneralMethods.MakeQuerySqlCommand(connectionNameEnum, query, ConnectionTimeout, Parameters);
            using (var result = com.ExecuteReader())
            {
                try
                {
                    if (typeof(T).IsValueType || "".GetType() == typeof(T))
                    {
                        list = new List<T>();
                        Parallel.ForEach(SimpleParallelDataReader<T>(result), (data) =>
                        {
                            list.Add(data);
                        });
                        return list;
                    }

                    list = new GeneralMethods().ToViewModel<T>(result);
                }
                catch (DataException)
                {
                    Parameters.Clear();
                    throw;
                }
                finally
                {
                    ConnectionManager.ReleaseConnection(com.Connection);
                }
            }
            return list;
        }

        public DataSet RunQuery(string connectionString, string query)
        {
            SqlCommand com = null;
            try
            {
                com = GeneralMethods.MakeQuerySqlCommand(connectionString, query, ConnectionTimeout, Parameters);
                var set = new DataSet();
                new SqlDataAdapter(com).Fill(set);

                return set;
            }
            catch (Exception)
            {
                {
                }
                throw;
            }
            finally
            {
                if (com != null)
                    ConnectionManager.ReleaseConnection(com.Connection);
            }

        }

        public DataTable GetTable(string connectionString, string tableName, string whereCondition)
        {
            SqlCommand com = null;
            try
            {
                com = GeneralMethods.MakeQuerySqlCommand(connectionString, "Select * from " + tableName + " " + whereCondition, ConnectionTimeout);
                var set = new DataSet();
                new SqlDataAdapter(com).Fill(set);

                return set.Tables[0];
            }
            catch (Exception)
            {
                {
                }
                throw;
            }
            finally
            {
                if (com != null)
                    ConnectionManager.ReleaseConnection(com.Connection);
            }
        }

        public List<T> GetTable<T>(ConnectionNameEnum connectionNameEnum, string tableName, string whereCondition)
        {
            List<T> list;
            var com = GeneralMethods.MakeQuerySqlCommand(connectionNameEnum, "Select * from " + tableName + " " + whereCondition, ConnectionTimeout);

            try
            {
                using (var result = com.ExecuteReader())
                {
                    if (typeof(T).IsValueType || "".GetType() == typeof(T))
                    {
                        list = new List<T>();
                        Parallel.ForEach(SimpleParallelDataReader<T>(result), (data) =>
                        {
                            list.Add(data);
                        });
                        return list;
                    }

                    list = new GeneralMethods().ToViewModel<T>(result);
                }
            }
            catch (DataException)
            {
                Parameters.Clear();
                throw;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(com.Connection);
            }

            return list;
        }



        public IList RunQuery(string assemblyName, string typeName, ConnectionNameEnum connectionNameEnum, string query)
        {
            MethodInfo method = typeof(StoreProcdureManagement).GetMethods().First(m => m.Name == "RunQuery" && m.GetGenericArguments().Length == 1);
            var typeArgs = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
            MethodInfo generic = method.MakeGenericMethod(typeArgs);
            return (IList)generic.Invoke(this, new[] { (object)connectionNameEnum, (object)query });
        }


        public void AddParameter(string key, object value)
        {
            Parameters.Add(new SqlParameter(key, value));
        }

        /// <summary>
        /// This method invike T properties and create parameter using it. 
        /// Note: All of parameters will begin with a '@' character.
        /// </summary>
        /// <typeparam name="T">Parameter object type</typeparam>
        /// <param name="obj">The parameter object.</param>
        /// <param name="ignorePropertiesList">A list of parameter should ignored</param>
        public void AddParameter<T>(T obj, List<string> ignorePropertiesList = null)
        {
            foreach (var prop in Activator.CreateInstance<T>().GetType().GetProperties())
            {
                if (ignorePropertiesList != null && ignorePropertiesList.Contains(prop.Name))
                    continue;
                Parameters.Add(new SqlParameter("@" + prop.Name, prop.GetValue(obj)));
            }

        }

        public List<T> RunSp<T>(ConnectionNameEnum connectionNameEnum, string storeProcureName,
            int recordCount, int pageNumber, out int allRecordCount)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            List<T> list;
            var com = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);
            var dt = new DataTable();

            try
            {
                using (var result = com.ExecuteReader())
                {
                    var schemaTable = result.GetSchemaTable();
                    if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                    {
                        var sqlSpException = new DataException();
                        //sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        while (result.Read())
                        {
                            sqlSpException.Add((int)result["ErrorCode"], result["ErrorMessage"].ToString());
                        }
                        throw sqlSpException;
                    }


                    if (typeof(T).IsValueType || "".GetType() == typeof(T))
                    {
                        list = new List<T>();
                        Parallel.ForEach(SimpleParallelDataReader<T>(result), (data) =>
                        {
                            list.Add(data);
                        });
                        allRecordCount = list.Count;
                        return list;
                    }

                    return new GeneralMethods().ToViewModel<T>(result, recordCount, pageNumber, out allRecordCount);
                    //dt.Load(result);
                    ////var tempTable = dt.n
                    ////for(int i = 0; i <= )


                    //var l = dt.AsEnumerable(); //.Select();
                    //allRecordCount = l.Count();
                    //var newList = l.Skip((pageNumber - 1)*recordCount).Take(recordCount);
                    //list = new GeneralMethods().ToViewModel<T>(newList);
                }
            }
            catch (DataException)
            {
                Parameters.Clear();
                throw;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(com.Connection);
            }


        }

        //public IList RunSp(ConnectionNameEnum connectionNameEnum, string storeProcureName, int recordCount, int pageNumber, out int allRecordCount,
        //    string assemblyName, string typeName)
        //{
        //    object allRecoreds = 0;
        //    MethodInfo method = typeof(StoreProcdureManagement).GetMethods().First(m=>m.Name == "RunSp" && m.GetGenericArguments().Length == 1 && m.GetParameters().Count() == 5);
        //    var typeArgs = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
        //    MethodInfo generic = method.MakeGenericMethod(typeArgs);
        //    var list = (IList)generic.Invoke((object)0, new[] { (object)connectionNameEnum, (object)storeProcureName, (object)recordCount, (object)pageNumber, (object)allRecoreds});
        //    allRecordCount = allRecoreds.ToInt();
        //    return list;
        //}


        /// <summary>
        /// Runs the sp.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns>Scalar data of sp</returns>
        /// <exception>SqlSPException ex
        ///     <cref>SqlSpException ex</cref>
        /// </exception>
        public object RunSp(string connectionString, string storeProcureName)
        {
            _ConnectionString = connectionString;
            return RunSp(ConnectionNameEnum.DirectConnectionString, storeProcureName);

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
        public object RunSp(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            object returnedObject = null;

            SqlCommand command;
            if (connectionNameEnum == ConnectionNameEnum.DirectConnectionString)
                command = new GeneralMethods().MakeSqlCommand(_ConnectionString, storeProcureName, param, ConnectionTimeout);
            else
                command = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);

            try
            {
                if (param.Any(p => p.Direction == ParameterDirection.Output))
                {
                    command.ExecuteNonQuery();
                    returnedObject = param.First(p => p.Direction == ParameterDirection.Output).Value;
                }
                else
                {
                    var dr = command.ExecuteReader();
                    if (dr.Read())
                    {
                        var schemaTable = dr.GetSchemaTable();
                        if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                        {
                            var sqlSpException = new DataException();
                            //sqlSpException.Add((int)dr["ErrorCode"], dr["ErrorMessage"].ToString());
                            sqlSpException.Add(int.Parse(dr["ErrorCode"].ToString()), dr["ErrorMessage"].ToString());
                            while (dr.Read())
                            {
                                sqlSpException.Add(int.Parse(dr["ErrorCode"].ToString()), dr["ErrorMessage"].ToString());
                            }

                            ConnectionManager.ReleaseConnection(command.Connection);
                            Parameters.Clear();
                            throw sqlSpException;
                        }
                        returnedObject = dr[0];
                    }
                }
            }
            catch (Exception ex)
            {
                var exp = new DataException();
                exp.Data.Add("Error", ex);
                throw exp;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(command.Connection);
            }

            return returnedObject;
        }

        public async Task<object> RunSpAsync(string connectionString, string storeProcureName)
        {
            _ConnectionString = connectionString;
            return await RunSpAsync(ConnectionNameEnum.DirectConnectionString, storeProcureName);

        }

        public async Task<object> RunSpAsync(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);

            Parameters.Clear();
            object returnedObject = null;

            SqlCommand command;
            if (connectionNameEnum == ConnectionNameEnum.DirectConnectionString)
                command = new GeneralMethods().MakeSqlCommand(_ConnectionString, storeProcureName, param, ConnectionTimeout);
            else
                command = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);

            try
            {
                if (param.Any(p => p.Direction == ParameterDirection.Output))
                {
                    command.ExecuteNonQuery();
                    returnedObject = param.First(p => p.Direction == ParameterDirection.Output).Value;

                }
                else
                {
                    var dr = await command.ExecuteReaderAsync();
                    if (dr.Read())
                    {
                        var schemaTable = dr.GetSchemaTable();
                        if (schemaTable != null && schemaTable.Select("ColumnName='ErrorCode'").Length > 0)
                        {
                            var sqlSpException = new DataException();
                            //sqlSpException.Add((int)dr["ErrorCode"], dr["ErrorMessage"].ToString());
                            sqlSpException.Add(int.Parse(dr["ErrorCode"].ToString()), dr["ErrorMessage"].ToString());
                            while (dr.Read())
                            {
                                sqlSpException.Add(int.Parse(dr["ErrorCode"].ToString()), dr["ErrorMessage"].ToString());
                            }

                            ConnectionManager.ReleaseConnection(command.Connection);
                            Parameters.Clear();
                            throw sqlSpException;
                        }
                        returnedObject = dr[0];
                    }
                }
            }
            catch (Exception ex)
            {
                var exp = new DataException();
                exp.Data.Add("Error", ex);
                throw exp;
            }
            finally
            {
                ConnectionManager.ReleaseConnection(command.Connection);
            }
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
        public List<object> RunObjectListReturnedSp<T>(ConnectionNameEnum connectionNameEnum,
            string storeProcureName)
        {
            List<object> list;
            try
            {
                var innerList = RunSp<T>(connectionNameEnum, storeProcureName);
                list = innerList.Cast<object>().ToList();
            }
            catch (DataException ex)
            {
                Parameters.Clear();
                throw ex;
            }
            return list;
        }



        /// <summary>
        /// Runs the sp returned table.
        /// </summary>
        /// <param name="connectionNameEnum">The connection name enum.</param>
        /// <param name="storeProcureName">Name of the store procure.</param>
        /// <returns></returns>
        public DataTable RunSpReturnedTable(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);
            Parameters.Clear();
            var command = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);
            try
            {

                var dr = command.ExecuteReader();
                var dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorCode"))
                    {
                        var sqlSpException = new DataException();
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            sqlSpException.Add((int)dt.Rows[i]["ErrorCode"], dt.Rows[i]["ErrorMessage"].ToString());
                        }
                        ConnectionManager.ReleaseConnection(command.Connection);
                        Parameters.Clear();
                        throw sqlSpException;
                    }

                    return dt;
                }
            }
            finally
            {

                ConnectionManager.ReleaseConnection(command.Connection);
                Parameters.Clear();
            }

           
            return null;
        }

        public DataTable RunSpReturnedDirectTable(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);
            Parameters.Clear();
            var command = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);



            try
            {
                var da = new SqlDataAdapter(command);
                var dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    if (dt.Columns["ErrorCode"] != null)
                    {
                        var sqlSpException = new DataException();
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            sqlSpException.Add((int)dt.Rows[i]["ErrorCode"], dt.Rows[i]["ErrorMessage"].ToString());
                        }
                        Parameters.Clear();
                        throw sqlSpException;
                    }
                return dt;
            }
            catch
            {
                return new DataTable();
            }
            finally
            {
                ConnectionManager.ReleaseConnection(command.Connection);
                Parameters.Clear();
            }
        }

        public DataSet RunSpReturnedDirectDataSet(ConnectionNameEnum connectionNameEnum, string storeProcureName)
        {
            var param = Parameters.Clone(); //.ToDictionary(x => x.Key, x => x.Value);
            Parameters.Clear();
            var command = new GeneralMethods().MakeSqlCommand(connectionNameEnum, storeProcureName, param, ConnectionTimeout);
            try
            {
                var da = new SqlDataAdapter(command);
                var dt = new DataSet();
                da.Fill(dt);

                return dt;
            }
            catch
            {
                return new DataSet();
            }
            finally
            {
                ConnectionManager.ReleaseConnection(command.Connection);
                Parameters.Clear();

            }
        }

        private static IEnumerable<T> SimpleParallelDataReader<T>(SqlDataReader reader)
        {
            using (reader)
            {
                if (reader.IsClosed) yield break;
                while (reader.HasRows && reader.Read())
                {
                    yield return (T)reader[0];
                }
            }
        }

        private static IEnumerable<dynamic> DynamicParallelDataReader(SqlDataReader reader, DataTable shemaTable)
        {
            using (reader)
            {
                if (reader.IsClosed) yield break;
                while (reader.HasRows && reader.Read())
                {
                    dynamic expando = new ExpandoObject();
                    var obj = (IDictionary<string, object>)expando;
                    for (var i = 0; i < shemaTable.Rows.Count; i++)
                        obj[shemaTable.Rows[i][0].ToString()] = reader[shemaTable.Rows[i][0].ToString()];
                    yield return expando;
                }
            }
        }
    }
}
