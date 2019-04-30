using System.Collections.Generic;
using System.Data;
using Shoniz.Common.Data.DataConvertor.Mapper;

namespace Shoniz.Common.Data.DataConvertor
{
    /// <summary>
    /// This class is used to convert DataTable to a list of a generic type. It will create and call after creating the convertor method.
    /// </summary>
    public class DataTableToList
    {
        /// <summary>
        /// Converts the specified Data Table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public List<T> Convert<T>(DataTable dt)
        {
            var list = new List<T>();
            if (dt.Rows.Count > 0)
            {
                IMapper<DataRow, T> mapperObject = new MapDispatcher().GetNewDataTableConvertorObject<T>(dt.Rows[0]);

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(mapperObject.Convert(dt.Rows[i]));
                }
            }
            return list;
        }
    }
}
