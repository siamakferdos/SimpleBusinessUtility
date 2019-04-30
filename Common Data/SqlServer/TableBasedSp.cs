using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Shoniz.Common.Core.Exception;
using Exception = System.Exception;

namespace Shoniz.Common.Data.SqlServer
{
    public class TableBasedSp
    {
        private string _runSpFullName;
        public TableBasedSp(string runSpFullName)
        {
            _runSpFullName = runSpFullName;
        }
        public DataSet RunSp(string spName, string connectionString, Dictionary<string, string> parameters = null)
        {
            var getUserDefinedColumnsQuery = @"select c.Name  from sys.table_types tt
                inner join sys.columns c on c.object_id = tt.type_table_object_id
                where tt.name = 'Parameters'  order by c.column_id";
            var tableList = new StoreProcdureManagement().RunQuery(connectionString, getUserDefinedColumnsQuery).Tables;
            if(tableList.Count < 1) return new DataSet();
            var paramTable = new DataTable();
            //paramTable.Columns.Add("Type", typeof(string));
            //paramTable.Columns.Add("Name", typeof(string));
            //paramTable.Columns.Add("Value", typeof(string));

            for(var i = 0; i < tableList[0].Rows.Count; i++)
                paramTable.Columns.Add(tableList[0].Rows[i]["Name"].ToString(), typeof(string));
            //paramTable.Columns.Add("Type", typeof(string));
            //paramTable.Columns.Add("Value", typeof(string));

            var connection = new SqlConnection { ConnectionString = connectionString };
            var command = new SqlCommand(_runSpFullName, connection) { CommandType = CommandType.StoredProcedure };
            command.CommandTimeout = 3600;
            var row = paramTable.NewRow();
            row["Name"] = "_SpName";
            row["Value"] = spName;
            row["Type"] = "";

           
            paramTable.Rows.Add(row);
            
            if (parameters != null)
                foreach (var parameter in parameters)
                {
                    row = paramTable.NewRow();
                    row["Name"] = parameter.Key;
                    row["Type"] = "";
                    row["Value"] = parameter.Value;
                    paramTable.Rows.Add(row);
                }


            command.Parameters.AddWithValue("@params", paramTable);

            //var param = new SqlParameter("@params", paramTable);
            //command.Parameters.Add(param);

            //command.Parameters.Add("@params", SqlDbType.Structured);
            //command.Parameters["@params"].Value = paramTable;

            try
            {
                var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables.Count > 1)
                    for (var i = 0; i < dataSet.Tables.Count - 1; i++)
                    {
                        var tableName = "";
                        if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[i]["Name"] != null && dataSet.Tables[dataSet.Tables.Count - 1].Rows[i]["Name"].ToString() != "")
                            tableName = dataSet.Tables[dataSet.Tables.Count - 1].Rows[i]["Name"].ToString();
                        else
                            switch (dataSet.Tables[dataSet.Tables.Count - 1].Rows[i]["TableType"].ToString())
                            {
                                case "M":
                                    tableName = "Message";
                                    break;
                                case "S":
                                    tableName = "Schema";
                                    break;
                                case "E":
                                    var sqlSpException = new DataException();
                                    sqlSpException.Add(dataSet.Tables[i]);
                                    throw sqlSpException;
                            }
                        dataSet.Tables[i].TableName = tableName;

                    }
                dataSet.Tables[dataSet.Tables.Count - 1].TableName = "TableDefination";
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable GetFirstTableOfData(string spName, string connectionString,
            Dictionary<string, string> parameters = null)
        {
            var exception = new DataException();
            try
            {
                var dataSet = RunSp(spName, connectionString, parameters);
                var defTable = dataSet.Tables[dataSet.Tables.Count - 1];

                for (var i = 0; i < defTable.Rows.Count; i++)
                {
                    if (defTable.Rows[i]["TableType"].ToString() == "D")
                    {
                        return dataSet.Tables[i];
                    }
                    if (defTable.Rows[i]["TableType"].ToString() == "S")
                    {
                        return dataSet.Tables[i];
                    }
                    if (defTable.Rows[i]["TableType"].ToString() != "E") continue;

                    
                    exception.Data.Add("ErrorTable", dataSet.Tables[i]);
                    throw exception;
                }
                return null;
            }
            catch
            {
                exception.Add(0, ExceptionStoredMessage.NotAppliedShonizStandard);
                throw exception;
            }
        }

        public DataSet GetAllTablesOfData(string spName, string connectionString,
            Dictionary<string, string> parameters = null, bool includeMessageTable = false)
        {


            var dataSet = RunSp(spName, connectionString, parameters);
            var tempDataset = new DataSet();
            var defTable = dataSet.Tables[dataSet.Tables.Count - 1];
            for (var i = 0; i < defTable.Rows.Count; i++)
            {
                DataTable generalAlertData = dataSet.Tables[0];
                dataSet.Tables.Remove(generalAlertData.TableName);
               
                if (defTable.Rows[i]["TableType"].ToString() == "D")
                {
                    tempDataset.Tables.Add(generalAlertData);
                }
                if (defTable.Rows[i]["TableType"].ToString() == "S")
                {
                    tempDataset.Tables.Add(generalAlertData);
                }
                if (defTable.Rows[i]["TableType"].ToString() == "M" && includeMessageTable)
                {
                    tempDataset.Tables.Add(generalAlertData);
                }

                if (defTable.Rows[i]["TableType"].ToString() != "E") continue;
                var exception = new DataException();
                exception.Data.Add("ErrorTable", dataSet.Tables[i]);
                throw exception;
            }
            return tempDataset;
        }

        //public List<T> ConvertToList<T>(DataTable dt)
        //{
        //    var list = new DataTableToList().Convert<T>(dt);
        //}

    }
}
