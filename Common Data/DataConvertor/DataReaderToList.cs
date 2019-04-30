using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Shoniz.Common.Data.DataConvertor.Mapper;

namespace Shoniz.Common.Data.DataConvertor
{
    /// <summary>
    /// This class is used to convert SqlDataReader to a list of a generic type. It will create and call after creating the convertor method.
    /// </summary>
    public class DataReaderToList
    {
        private static SqlDataReader _reader;
        /// <summary>
        /// Converts the specified reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public List<T> Convert<T>(SqlDataReader reader)
        {
            _reader = reader;
            var list = new List<T>();
            //while (reader.Read())
            //{
            //    list.Add(mapperObject.Convert(reader));
            //}
            Parallel.ForEach(ReadData<T>(), (data) =>
            {
                    list.Add(data);
            });

            return list;
        }

        private static IEnumerable<T> ReadData<T>()
        {
            var mapperObject = new MapDispatcher().GetNewDataReaderConvertorObject<T>(_reader);
            using (_reader)
            {
                if(_reader.IsClosed) yield break;
                while (_reader.HasRows && _reader.Read())
                {
                    yield return mapperObject.Convert(_reader);
                }
            }
        }
    }
}
