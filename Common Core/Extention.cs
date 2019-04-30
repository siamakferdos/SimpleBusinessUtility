using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace Shoniz.Common.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the MD5 value.
        /// </summary>
        /// <param name="userPass">The user pass.</param>
        /// <returns></returns>
        public static Byte[] GetMd5Value(this string userPass)
        {
            var bytePass = System.Text.Encoding.UTF8.GetBytes(userPass);
            var md5Srv = new MD5CryptoServiceProvider();
            return md5Srv.ComputeHash(bytePass);
        }

        /// <summary>
        /// To the MD5 encoding value.
        /// </summary>
        /// <param name="userPass">The user pass.</param>
        /// <returns>the MD5 coded byte array</returns>
        public static Byte[] ToMd5Value(String userPass)  
        {
            var bytePass = System.Text.Encoding.UTF8.GetBytes(userPass);
            var md5Srv = new MD5CryptoServiceProvider();
            return md5Srv.ComputeHash(bytePass);
        }



        /// <summary>
        /// Make a string to the object variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Empty string if null object or couldn't convert to string. 
        /// This will return string of object on seccess</returns>
        public static string ToStringVar(this object obj)
        {
            if (obj == null)
                return "";

            try { return obj.ToString(); }
            catch { return ""; }

        }

        /// <summary>
        /// Make a string to the decimal variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Empty string if null object or couldn't convert to string. 
        /// This will return string of object on seccess</returns>
        public static string ToStringVar(this decimal? obj)
        {
            if (obj.HasValue)
            {
                try { return obj.ToString(); }
                catch { return ""; }
            }
            return "";
        }

        /// <summary>
        /// Make a string to the Integer variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Empty string if null object or couldn't convert to string. 
        /// This will return string of object on seccess</returns>
        public static string ToStringVar(this int? obj)
        {
            if (obj.HasValue)
            {
                try { return obj.ToString(); }
                catch { return ""; }
            }
            return "";
        }

        /// <summary>
        /// Make a string to the float variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Empty string if null object or couldn't convert to string. 
        /// This will return string of object on seccess</returns>
        public static string ToStringVar(this float? obj)
        {
            if (obj.HasValue)
            {
                try { return obj.ToString(); }
                catch { return ""; }
            }
            return "";
        }

        /// <summary>
        /// Make an Integer of the string variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>-1 on failur convert.
        /// This will return truly Integer on seccess</returns>
        public static int ToInt(this string obj)
        {
            try
            {
                return int.Parse(obj);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Make an Integer of the object variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>-1 on failur convert.
        /// This will return truly Integer on seccess</returns>
        public static int ToInt(this object obj)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Determines whether [is int number] [the specified object].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsIntNumber(this object obj)
        {
            try
            {
                int.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified object is number.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsDoubleNumber(this object obj)
        {
            try
            {
                double.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Make an Float of the string variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>-1 on failur convert.
        /// This will return truly Float on seccess</returns>
        public static float ToFloat(this string obj)
        {
            try
            {
                return float.Parse(obj);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Make an Float of the object variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>-1 on failur convert.
        /// This will return truly float on seccess</returns>
        public static float ToFloat(this object obj)
        {
            try
            {
                return float.Parse(obj.ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Make an Float of the object variable. 
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>-1 on failur convert.
        /// This will return truly float on seccess</returns>
        public static string TrimZeroDecimal(this object obj)
        {
            var str = obj.ToStringVar();
            var removeCharStartIndex = str.Length;
            if (str.IndexOf('/') < 1 && str.IndexOf('.') < 1)
                return obj.ToString();

            while (str[str.Length - 1] == '0')
                str = str.Remove(str.Length - 1);

            if (str[str.Length - 1] == '.' || str[str.Length - 1] == '/')
                str = str.Remove(str.Length - 1);
                
            //for (int i = obj.ToString().Length - 1; i >= 0; i--)
            //{
            //    if (str[i] == '0') removeCharStartIndex = i;
            //    else if (str[i] == '/' || str[i] == '.')
            //    {
            //        removeCharStartIndex = i;
            //        break;
            //    }
            //    else break;
            //}
            //if(removeCharStartIndex < str.Length)
            //    return str.Remove(removeCharStartIndex);
            return str;
        }



        /// <summary>
        /// convert a DateTime to persian date in this format : 
        /// <para>1393/05/09</para>
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns></returns>
        public static string ToPersianDate(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(date).ToString("0000"),
                pc.GetMonth(date).ToString("00"), pc.GetDayOfMonth(date).ToString("00"));
        }

        public static string ToPersianYear(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return pc.GetYear(date).ToString("0000");
        }

        public static string ToPersianMonth(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return pc.GetMonth(date).ToString("00");
        }
        public static string ToPersianDay(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return pc.GetDayOfMonth(date).ToString("00");
        }


        public static string ToPersianDateTime(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return string.Format("{0}/{1}/{2} {3}", pc.GetYear(date).ToString("0000"),
                pc.GetMonth(date).ToString("00"), pc.GetDayOfMonth(date).ToString("00"), date.TimeOfDay);
        }

        public static System.Data.DataTable ToTableValuedParameter<T, TProperty>(this System.Collections.Generic.IEnumerable<T> list,
            Func<T, TProperty> selector)
        {
            var tbl = new System.Data.DataTable();
            tbl.Columns.Add("Id", typeof(T));

            foreach (var item in list)
            {
                tbl.Rows.Add(selector.Invoke(item));

            }
            return tbl;
        }

        public static Dictionary<string, object> ToDictionary<T>(this T obj)
        {
            var dic = new Dictionary<string, object>();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    var val = propertyInfo.GetValue(obj) ?? "";
                    dic.Add(propertyInfo.Name, val);
                }
            }
            return dic;
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static List<T> ToViewModel<T>(this DataTable dt)
        {
            var list = new List<T>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var o = (T)Activator.CreateInstance(typeof(T));
                var fields = o.GetType().GetFields();
                new List<PropertyInfo>();
                foreach (var prop in o.GetType().GetProperties())
                {
                    try
                    {
                        prop.SetValue(o, dr[prop.Name], null);
                    }
                    catch
                    {
                        try
                        {
                            var o2 = Activator.CreateInstance(prop.PropertyType);
                            foreach (var propProp in prop.PropertyType.GetProperties())
                            {
                                try
                                {
                                    propProp.SetValue(o2, dr[propProp.Name], null);
                                }
                                catch { }
                            }
                            prop.SetValue(o, o2, null);
                        }
                        catch
                        {
                        }
                    }
                }
                if (fields.Any())
                {
                    foreach (var f in fields)
                    {
                        foreach (var p in f.FieldType.GetProperties())
                        {
                            try
                            {
                                p.SetValue(f.GetValue(o), dr[p.Name], null);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                list.Add(o);
            }
            return list;
        }

        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
    }
}
