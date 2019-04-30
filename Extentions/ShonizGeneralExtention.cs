using System;
using System.Security.Cryptography;

namespace Extentions
{
    public static class Extensions
    {
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
            {
                return "";
            }
            else
            {
                try { return obj.ToString(); }
                catch { return ""; }
            }
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
    }
}
