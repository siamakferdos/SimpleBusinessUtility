using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;


namespace Shoniz.Common.Web
{
    public static class WebExtention
    {
        /// <summary>
        /// Converts a string that split with some Separator to dictionary. Like key:val|key:val|key:val
        /// </summary> 
        /// <param name="input">The input.</param>
        /// <param name="skipSeparators">The skiped separators.</param>
        /// <returns> Dictionary<string, object></returns>
        public static Dictionary<string, object> ConvertToDictionary(this string input, string skipSeparators = "")
        {
            var dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            char firstSeparator = '\0', secondSeparator = '\0';
            int firstSeparatorPlace = -1;
            char[] separators = { '-', '_', '+', '=', '|', ':', ';', '/', '\\', '?', '>', '<', ',', '&', '^', '%', '$', '#', '@', '!' };
            byte separatorCount = 0;
            separators = separators.Where(c => !skipSeparators.Contains(c)).ToArray();
            foreach (char ch in separators)
            {
                var index = input.IndexOf(ch);
                if (index < 0) continue;
                if (separatorCount == 0)
                {
                    firstSeparator = ch;
                    firstSeparatorPlace = index;
                    separatorCount++;
                }
                else
                {
                    if (firstSeparatorPlace > index)
                    {
                        secondSeparator = firstSeparator;
                        firstSeparator = ch;
                        separatorCount++;
                        break;
                    }
                    secondSeparator = ch;
                    separatorCount++;
                    break;
                }
            }
            if (separatorCount < 1)
                return dic;
            if (separatorCount > 1)
            {
                foreach (var keyValue in input.Split(secondSeparator))
                {
                    dic.Add(keyValue.Split(firstSeparator)[0], keyValue.Split(firstSeparator)[1]);
                }
            }
            else
            {
                dic.Add(input.Split(firstSeparator)[0], input.Split(firstSeparator)[1]);
            }
            
            return dic;
        }
        /// <summary>
        /// Converts a string that split with some Separator to dictionary. Like key:val|key:val|key:val
        /// </summary> 
        /// <param name="input">The input.</param>
        /// <param name="skipSeparators">The skiped separators.</param>
        /// <returns> Dictionary<string, object></returns>
        public static List<SqlParameter> ConvertToSqlParameters(this string input, string skipSeparators = "")
        {
            var dic = new List<SqlParameter>();
            char firstSeparator = '\0', secondSeparator = '\0';
            int firstSeparatorPlace = -1;
            char[] separators = { '-', '_', '+', '=', '|', ':', ';', '/', '\\', '?', '>', '<', ',', '&', '^', '%', '$', '#', '@', '!' };
            byte separatorCount = 0;
            separators = separators.Where(c => !skipSeparators.Contains(c)).ToArray();
            foreach (char ch in separators)
            {
                var index = input.IndexOf(ch);
                if (index < 0) continue;
                if (separatorCount == 0)
                {
                    firstSeparator = ch;
                    firstSeparatorPlace = index;
                    separatorCount++;
                }
                else
                {
                    if (firstSeparatorPlace > index)
                    {
                        secondSeparator = firstSeparator;
                        firstSeparator = ch;
                        separatorCount++;
                        break;
                    }
                    secondSeparator = ch;
                    separatorCount++;
                    break;
                }
            }
            if (separatorCount < 1)
                return dic;
            if (separatorCount > 1)
            {
                foreach (var keyValue in input.Split(secondSeparator))
                {
                    dic.Add(new SqlParameter(keyValue.Split(firstSeparator)[0], keyValue.Split(firstSeparator).Count() > 1 ? keyValue.Split(firstSeparator)[1] : ""));
                }
            }
            else
            {
                dic.Add(new SqlParameter(input.Split(firstSeparator)[0], input.Split(firstSeparator)[1]));
            }

            return dic;
        }

        /// <summary>
        /// Convert a string in a format like a1,a2,a3,a4 To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="skipSeparators">The skip separators.</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string input, string skipSeparators = "")
        {
            char[] separators = { '-', '_', '+', '=', '|', ':', ';', '/', '\\', '?', '>', '<', ',', '&', '^', '%', '$', '#', '@', '!' };

            separators = separators.Where(c => !skipSeparators.Contains(c)).ToArray();
            var separator = new char();
            foreach (var ch in separators)
            {
                var index = input.IndexOf(ch);
                if (index <= 0) continue;
                separator = ch;
            }

            var list = input.Split(separator).Select(str => (T) Convert.ChangeType(str, typeof (T))).ToList();

            return list;
        }

        /// <summary>
        /// Convert a dictionary to the query string in this format : 
        /// <para>key:val|key:val|key:val</para>
        /// </summary>
        /// <param name="dic">The dictionary</param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, object> dic)
        {
            if (dic == null || dic.Count <= 0) return "";

            var result = "";
            foreach (var d in dic)
            {
                if (result != "")
                    result += "|";
                result += d.Key + ":" + d.Value;
            }
            return result;
        }

        public static string ToQueryString(this List<SqlParameter> dic)
        {
            if (dic == null || dic.Count <= 0) return "";
            var result = "";
            foreach (SqlParameter d in dic)
            {
                if (result != "")
                    result += "|";
                result += d.ParameterName + ":" + d.Value;
            }
            return result;
        }

        public static Dictionary<string, List<string>> ConvertJsonToDictionary(this string json)
        {
            json = json.Trim();
            try
            {
                var splitted = json.Split(new string[] {"},"}, StringSplitOptions.None);
                
                var dic = new Dictionary<string, List<string>>();

                foreach (var row in splitted)
                {
                    var k = row.Replace("{", "").Replace("}", "");
                    var keyValue = k.Split(':');
                    dic.Add(keyValue[0].ToString(), keyValue[1].Split(',').ToList());
                }
                return dic;
            }
            catch
            {
                throw new Exception("Wrong format json string passed to this method");
            }
        }
    }
}
