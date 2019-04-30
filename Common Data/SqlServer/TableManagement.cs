using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Core;

namespace Shoniz.Common.Data.SqlServer
{
    public class TableManagement
    {
        private readonly List<Filter> _filters = new List<Filter>();
        private readonly Dictionary<string, string> _customColumn = new Dictionary<string, string>();

        public List<T> GetTable<T>(string connectionString, string tableName = "fromT")
        {
            var db = new StoreProcdureManagement();
            var _tableName = tableName;
            if (tableName == "fromT")
            {
                _tableName = typeof(T).Name;
            }

            var param = "";
            var counter = 0;

            var whereCondition = "";
            lock (_filters)
            {
                foreach (var f in _filters)
                {
                    if (whereCondition != "")
                        whereCondition += " AND ";
                    param = "@param" + counter;
                    counter++;

                    whereCondition += f.ColumnName + " " + GetOprand(f.WhereFilter) + " " + param;
                    db.AddParameter(param, f.Value);
                }
                _filters.Clear();
            }
            if (whereCondition != "")
                whereCondition = " WHERE " + whereCondition;

            var columns = "";
            lock (_customColumn)
            {
                foreach (var col in _customColumn)
                {
                    if (columns != "")
                        columns += ", ";
                    columns += " " + col.Key + " " + col.Value;
                }
                _customColumn.Clear();
            }

            return db.RunQuery<T>(connectionString,
                "SELECT " + (columns == "" ? "*" : columns)
                + " FROM " + _tableName + whereCondition);
        }
        public void AddFilters(string columnName, WhereFilter filter, string value)
        {
            _filters.Add(new Filter
            {
                WhereFilter = filter,
                ColumnName = columnName,
                Value = value
            });
        }

        public void AddCustomColumn(string tableColName, string alis = "")
        {
            _customColumn.Add(tableColName, alis);
        }

        private string GetOprand(WhereFilter filter)
        {
            switch (filter)
            {
                case WhereFilter.BigThan: return " > ";
                case WhereFilter.BigOrEqual: return " >= ";
                case WhereFilter.Equal: return " = ";
                case WhereFilter.NotEqual: return " <> ";
                case WhereFilter.SmallOrEqual: return " <= ";
                case WhereFilter.SmallThan: return " > ";
            }
            return "";
        }

        public void InsertToTable(string connectionString, object obj, string tableName = "fromObjectName",
            Dictionary<string, string> replacements = null,
            System.Collections.Generic.List<string> ignoreList = null)
        {
            var db = new StoreProcdureManagement();
            if (tableName == "fromObjectName")
                tableName = obj.GetType().Name;

            var sqlCommandString = "Select TOP 1 * FROM " + tableName;
            var dataset = db.RunQuery(connectionString, sqlCommandString);
            var table = dataset.Tables[0];

            var dic = obj.ToDictionary();

            if (replacements != null)
                foreach (var newName in replacements)
                {
                    if (dic.Keys.Contains(newName.Key))
                    {
                        dic.Add(newName.Value, dic[newName.Key]);
                        dic.Remove(newName.Key);
                    }
                }

            sqlCommandString = "INSERT INTO " + tableName;
            var fields = "";
            var values = "";
            var valueList = new List<string>();
            var counter = 0;
            var sqlParameter = new List<SqlParameter>();
            foreach (var record in dic)
            {
                if (ignoreList == null || !ignoreList.Contains(record.Key))
                {
                    if (!table.Columns.Contains(record.Key))
                        throw new Exception("فیلد '" + record.Key + "' در جدول وجود ندارد");

                    if (fields != "")
                    {
                        fields += ", ";
                        values += ",";
                    }
                    fields += record.Key;

                    var param = "@param" + counter;
                    values += param;

                    db.AddParameter(param, record.Value);
                    counter++;
                }
            }
            sqlCommandString += " (" + fields + ") VALUES(" + values + ")";
            db.RunQuery(connectionString, sqlCommandString);

        }


        public void UpdateTable(string connectionString, object obj, string tableName = "fromObjectName",
            Dictionary<string, string> replacements = null,
            System.Collections.Generic.List<string> ignoreList = null)
        {
            var db = new StoreProcdureManagement();
            if (tableName == "fromObjectName")
                tableName = obj.GetType().Name;

            var sqlCommandString = "Select TOP 1 * FROM " + tableName;
            var dataset = db.RunQuery(connectionString, sqlCommandString);
            var table = dataset.Tables[0];

            var dic = obj.ToDictionary();

            if (replacements != null)
                foreach (var newName in replacements)
                {
                    if (dic.Keys.Contains(newName.Key))
                    {
                        dic.Add(newName.Value, dic[newName.Key]);
                        dic.Remove(newName.Key);
                    }
                }

            sqlCommandString = "UPDATE " + tableName + " SET ";
            var fields = "";
            //var values = "";
            var valueList = new List<string>();
            var counter = 0;
            //var sqlParameter = new List<SqlParameter>();

            var primaryKeys = GetPrimaryKeys(connectionString, tableName);

            var param = "";
            foreach (var record in dic)
            {
                if ((ignoreList == null || !ignoreList.Contains(record.Key)) && primaryKeys.All(r => r != record.Key))
                {
                    if (!table.Columns.Contains(record.Key))
                        throw new Exception("فیلد '" + record.Key + "' در جدول وجود ندارد");

                    if (fields != "")
                    {
                        fields += ", ";
                        //values += ",";
                    }

                    param = "@param" + counter;
                    fields += record.Key + " = " + param;

                    db.AddParameter(param, record.Value);
                    counter++;
                }
            }
            sqlCommandString += fields;

            var whereCondition = "";
            lock (_filters)
            {
                foreach (var f in _filters)
                {
                    if (whereCondition != "")
                        whereCondition += " AND ";
                    param = "@param" + counter;
                    counter++;

                    whereCondition += f.ColumnName + " " + GetOprand(f.WhereFilter) + " " + param;
                    db.AddParameter(param, f.Value);
                }
                _filters.Clear();
            }
            if (whereCondition != "")
                whereCondition = " WHERE " + whereCondition;
            sqlCommandString += whereCondition;

            db.RunQuery(connectionString, sqlCommandString);
        }

        public void DeleteRow(string connectionString, string tableName)
        {
            var db = new StoreProcdureManagement();

            var sqlCommandString = "DELETE FROM " + tableName;

            var whereCondition = "";
            lock (_filters)
            {
                var counter = 0;
                foreach (var f in _filters)
                {
                    if (whereCondition != "")
                        whereCondition += " AND ";
                    var param = "@param" + counter;
                    counter++;

                    whereCondition += f.ColumnName + " " + GetOprand(f.WhereFilter) + " " + param;
                    db.AddParameter(param, f.Value);
                }
                _filters.Clear();
            }
            if (whereCondition != "")
                whereCondition = " WHERE " + whereCondition;
            sqlCommandString += whereCondition;

            db.RunQuery(connectionString, sqlCommandString);
        }

        private List<string> GetPrimaryKeys(string connectionString, string tableName)
        {
            var db = new StoreProcdureManagement();
            string command = "SELECT  " +
                             " COL_NAME(ic.OBJECT_ID, ic.column_id) AS ColumnName" +
                             " FROM sys.indexes AS i INNER JOIN" +
                             " sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID" +
                             " AND i.index_id = ic.index_id" +
                             " WHERE i.is_primary_key = 1 AND OBJECT_NAME(ic.OBJECT_ID) = '" + tableName + "'";
            return db.RunQuery<string>(connectionString, command);
        }
    }

    internal class Filter
    {
        public WhereFilter WhereFilter { get; set; }
        public string ColumnName { get; set; }
        public string Value { get; set; }

    }

    public enum WhereFilter
    {
        BigThan,
        SmallThan,
        BigOrEqual,
        SmallOrEqual,
        Equal,
        NotEqual
    }
}
