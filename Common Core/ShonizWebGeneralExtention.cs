using System;
using System.Collections.Generic;
using System.Linq;

namespace Shoniz.Extention
{
    public static class ShonizWebGeneralExtention
    {
        /// <summary>
        /// Converts a string that split with some Separator to dictionary. Like key:val|key:val|key:val
        /// </summary> 
        /// <param name="input">The input.</param>
        /// <param name="skipSeparators">The skiped separators.</param>
        /// <returns> Dictionary<string, object></returns>
        public static Dictionary<string, object> ConvertToDictionary(this string input, string skipSeparators = "")
        {
            var dic = new Dictionary<string, object>();
            char firstSeparator = '\0', secondSeparator = '\0';
            int firstSeparatorPlace = -1;
            char[] separators = { '-', '_', '+', '=', '|', ':', ';', '/', '\\', '?', '>', '<', ',', '&', '^', '%', '$', '#', '@', '!' };
            byte separatorCount = 0;
            separators = separators.Where(c => !skipSeparators.Contains(c)).ToArray();
            foreach (char ch in separators)
            {
                var index = input.IndexOf(ch);
                if (index <= 0) continue;
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
                        break;
                    }
                    secondSeparator = ch;
                    break;
                }
            }

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
        /// Convert a string in a format like a1,a2,a3,a4 To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="skipSeparators">The skip separators.</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string input, string skipSeparators = "")
        {
            var list = new List<T>();

            char[] separators = { '-', '_', '+', '=', '|', ':', ';', '/', '\\', '?', '>', '<', ',', '&', '^', '%', '$', '#', '@', '!' };

            separators = separators.Where(c => !skipSeparators.Contains(c)).ToArray();
            var separator = new char();
            foreach (var ch in separators)
            {
                var index = input.IndexOf(ch);
                if (index <= 0) continue;
                separator = ch;
            }

            foreach (var str in input.Split(separator))
            {
                list.Add((T)Convert.ChangeType(str, typeof(T)));
            }

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
    }
}
