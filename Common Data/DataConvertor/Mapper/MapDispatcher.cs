using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace Shoniz.Common.Data.DataConvertor.Mapper
{
    /// <summary>
    /// Diffrent kind of objects have various way to get the special part of it to create a convertor method which can iterate in its body. 
    /// For example for sql reader whole reader can be pass as source but for dataTable first row is good idea. 
    /// </summary>
    public class MapDispatcher
    {
        /// <summary>
        /// This method returns a IMapper object which has Convert method. The source of convertor is SqlDataReader object and target can be any 
        /// type.
        /// </summary>
        /// <typeparam name="T">A Generic Type</typeparam>
        /// <param name="reader">The SqlDataReader.</param>
        /// <returns>An object with convertor method</returns>
        public IMapper<SqlDataReader, T> GetNewDataReaderConvertorObject<T>(SqlDataReader reader)
        {
            var typeFullName = typeof (T).FullName;
            if (NewObjectPool.Classpool.ContainsKey(typeFullName))
                return (IMapper<SqlDataReader, T>) Activator.CreateInstance(NewObjectPool.Classpool[typeFullName]);
            
            var classGenerator = new DataReaderConvertorGenerator<T>();
            var type = classGenerator.ClassGenerator(reader);
            if (NewObjectPool.Classpool.ContainsKey(typeFullName))
                return (IMapper<SqlDataReader, T>) Activator.CreateInstance(NewObjectPool.Classpool[typeFullName]);
                NewObjectPool.Classpool.Add(typeFullName, type);
            return (IMapper<SqlDataReader, T>)Activator.CreateInstance(type);
        }

        public IMapper<SqlDataRecord, T> GetNewDataRecordConvertorObject<T>(SqlDataReader reader)
        {
            var typeFullName = typeof(T).FullName;
            if (NewObjectPool.Classpool.ContainsKey(typeFullName))
                return (IMapper<SqlDataRecord, T>)Activator.CreateInstance(NewObjectPool.Classpool[typeFullName]);

            var classGenerator = new DataReaderConvertorGenerator<T>();
            var type = classGenerator.ClassGenerator(reader);
            NewObjectPool.Classpool.Add(typeFullName, type);
            return (IMapper<SqlDataRecord, T>)Activator.CreateInstance(type);
        }

        /// <summary>
        /// This method returns a IMapper object which has Convert method. The source of convertor is DataRow object and target can be any 
        /// type.
        /// </summary>
        /// <typeparam name="T">A Generic Type</typeparam>
        /// <param name="dataRow">The DataRow.</param>
        /// <returns>An object with convertor method</returns>
        public IMapper<DataRow, T> GetNewDataTableConvertorObject<T>(DataRow dataRow)
        {
            var typeFullName = typeof(T).FullName;
            if (NewObjectPool.Classpool.ContainsKey(typeFullName))
                return (IMapper<DataRow, T>)Activator.CreateInstance(NewObjectPool.Classpool[typeFullName]);

            var classGenerator = new DataTableConvertorGenerator<T>();
            var type = classGenerator.ClassGenerator(dataRow);
            NewObjectPool.Classpool.Add(typeFullName, type);
            return (IMapper<DataRow, T>)Activator.CreateInstance(type);
        }

        //public IMapper<DataRow, T> GetNewGridRowConvertorObject<T>()
        //{
        //    var typeFullName = typeof(T).FullName;
        //    if (NewObjectPool.Classpool.ContainsKey(typeFullName))
        //        return (IMapper<DataRow, T>)Activator.CreateInstance(NewObjectPool.Classpool[typeFullName]);

        //    var classGenerator = new DataTableConvertorGenerator<T>();
        //    var type = classGenerator.ClassGenerator(T);
        //    NewObjectPool.Classpool.Add(typeFullName, type);
        //    return (IMapper<DataRow, T>)Activator.CreateInstance(type);
        //}
    }
}
