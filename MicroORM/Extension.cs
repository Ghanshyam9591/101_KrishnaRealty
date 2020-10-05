using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace System
{
    public static class StringExtension
    {

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string SearchInsert(this string value, string search, string insertValue)
        {
            int i = value.LastIndexOf(search);
            while (i != -1)
            {
                value = value.Insert(i, insertValue);
                i = value.LastIndexOf(search, i);
            }
            return value;
        }

        public static string PadLeftEx(this string value, int totalWidth)
        {
            return value.PadLeftEx(totalWidth, ' ');
        }

        public static string PadLeftEx(this string value, int totalWidth, char paddingChar)
        {
            if (value.Length > totalWidth)
                return value.Substring(0, totalWidth);
            else
                return value.PadLeft(totalWidth, paddingChar);
        }

        public static string PadRightEx(this string value, int totalWidth)
        {
            return value.PadRightEx(totalWidth, ' ');
        }

        public static string PadRightEx(this string value, int totalWidth, char paddingChar)
        {
            if (value.Length > totalWidth)
                return value.Substring(0, totalWidth);
            else
                return value.PadRight(totalWidth, paddingChar);
        }
        public static string AppendLine(this string obj, string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                if (obj == null)
                    obj = "";
                if (obj != "")
                    obj += "\r\n";
                obj += line;
            }
            return obj;
        }

    }

    public static class DbDataReaderExtensions
    {
        public static T Get<T>(this DbDataReader reader, string column)
        {
            if (reader[column] == DBNull.Value)
                return default(T);
            else
                return (T)reader[column];
        }

        public static string GetString(this DbDataReader reader, string column)
        {
            if (reader[column] == DBNull.Value)
                return null;
            else
                return reader[column].ToString();
        }
    }

}