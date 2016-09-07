using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WLX.Utils.WebUtils
{
    public static class DataConverter
    {
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.Trim();
                return (((string.Compare(strValue, "true", true) == 0) || (string.Compare(strValue, "yes", true) == 0)) || (string.Compare(strValue, "1", true) == 0));
            }
            return false;
        }
        /// <summary>
        /// string型转换为时间型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的时间类型结果</returns>
        public static DateTime StrToDateTime(object strValue, DateTime defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 40))
            {
                return defValue;
            }

            DateTime val;

            if (!DateTime.TryParse(strValue.ToString(), out val))
            {
                val = defValue;
            }
            return val;
        }
        /// <summary>
        /// 将输入的int型字符串转换为DateTime类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime StrIntToDateTime(string strInt)
        {
            return IntToDateTime(DataConverter.StrToInt(strInt, 0));
        }

        /// <summary>
        /// 将输入的字符串int型转换为DateTime类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime StrIntToDateTime(object strInt)
        {
            return StrIntToDateTime(strInt.ToString());
        }

        /// <summary>
        /// 将输入的字符串int型转换为DateTime字符串类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StrIntToStrDateTime(string strInt)
        {
            return StrIntToDateTime(DataConverter.StrToInt(strInt, 0)).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将输入的int型转换为DateTime型
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime IntToDateTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        /// <summary>
        /// 将输入的DateTime型转换为int型
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToInt(DateTime time)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = time.Subtract(dtStart);
            return (int)(toNow.Ticks / 10000000);
        }

        /// <summary>
        /// object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(object strValue)
        {
            if (!Convert.IsDBNull(strValue) && !object.Equals(strValue, null))
            {
                return StrToDecimal(strValue.ToString());
            }
            return 0M;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string strValue)
        {
            decimal num;
            decimal.TryParse(strValue, out num);
            return num;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string input, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(input, out num))
            {
                return num;
            }
            return defaultValue;
        }
        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(object strValue)
        {
            if (!Convert.IsDBNull(strValue) && !object.Equals(strValue, null))
            {
                return StrToDouble(strValue.ToString());
            }
            return 0.0;
        }
        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(string strValue)
        {
            double num;
            double.TryParse(strValue, out num);
            return num;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的float类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
            {
                return defValue;
            }

            float val = 0;
            if (float.TryParse(strValue.ToString(), out val))
            {
                return val;
            }
            return defValue;
        }


        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果.如果要转换的字符串是非数字,则返回0.</returns>
        public static int StrToInt(object strValue)
        {
            return StrToInt(strValue, 0);
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object strValue, int defValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
            {
                return defValue;
            }

            string val = strValue.ToString();
            int intValue = 0;
            if (int.TryParse(val, out intValue))
            {
                return intValue;
            }
            return defValue;
        }
        /// <summary>
        /// string型转换为Long型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Long类型结果</returns>
        public static long StrToLong(object strValue)
        {
            return StrToLong(strValue, 0);
        }

        /// <summary>
        /// string型转换为Long型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static long StrToLong(object strValue, long defValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
            {
                return defValue;
            }

            string val = strValue.ToString();
            long intValue = 0;
            if (long.TryParse(val, out intValue))
            {
                return intValue;
            }
            return defValue;
        }
    }
}
