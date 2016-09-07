using System;

namespace WLX.Utils.WebUtils
{
    public static class DateTimeExtensions
    {
        #region 私有方法
        /// <summary>
        /// 检查闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static bool CheckLeapYear(int year)
        {
            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获得指定月的天数
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private static int GetDays(DateTime dt)
        {
            switch (dt.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    if (DateTime.IsLeapYear(dt.Year))
                        return 29;
                    return 28;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
            }
            return 0;
        }


        #endregion

        #region 公共方法
        /// <summary>
        /// 得到月末一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dt)
        {
            int days = GetDays(dt);
            DateTime time = new DateTime(dt.Year, dt.Month, days);
            return time;
        }
        /// <summary>
        /// 得到月第一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dt)
        {
            DateTime time = new DateTime(dt.Year, dt.Month, 1);
            return time;
        }
        /// <summary>
        /// 得到周末一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(this DateTime dt)
        {
            int day = (int)dt.DayOfWeek;
            DateTime time = new DateTime(dt.Year, dt.Month, dt.Day);
            if (day > 0)
            {
                return time.AddDays(7 - day);
            }
            return dt;
        }
        /// <summary>
        /// 得到周第一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstdayOfWeek(this DateTime dt)
        {
            int day = (int)dt.DayOfWeek;
            DateTime time = new DateTime(dt.Year, dt.Month, dt.Day);
            if (day > 0)
            {
                return time.AddDays(1 - day);
            }
            else
            {
                return time.AddDays(0 - 6);
            }
        }

        /// <summary>
        /// 得到一日的开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetBeginTimeOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
        /// <summary>
        /// 得到一日的最后时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndTimeOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }
        
        /// <summary>
        /// 计算两个日期之间的天数
        /// </summary>
        /// <param name="beginDay">起始时间</param>
        /// <param name="endDay">结束时间</param>
        /// <returns></returns>
        public static double CalculateDays(this DateTime beginDay, DateTime endDay)
        {
            TimeSpan ts = endDay - beginDay;
            return ts.TotalDays;
        }
        /// <summary>
        /// 计算两个日期之间的秒数
        /// </summary>
        /// <param name="beginDay">起始时间</param>
        /// <param name="endDay">结束时间</param>
        /// <returns></returns>
        public static double CalculateSeconds(DateTime beginDay, DateTime endDay)
        {
            TimeSpan ts = endDay - beginDay;
            return ts.TotalSeconds;
        }
        /// <summary>
        /// 计算两个日期之间的分钟数
        /// </summary>
        /// <param name="beginDay">起始时间</param>
        /// <param name="endDay">结束时间</param>
        /// <returns></returns>
        public static double CalculateMinutes(this DateTime beginDay, DateTime endDay)
        {
            TimeSpan ts = endDay - beginDay;
            return ts.TotalMinutes;
        }
    
        /// <summary>
        /// 计算两个日期之间的小时数
        /// </summary>
        /// <param name="beginDay">起始时间</param>
        /// <param name="endDay">结束时间</param>
        /// <returns></returns>
        public static double CalculateHours(DateTime beginDay, DateTime endDay)
        {
            TimeSpan ts = endDay - beginDay;
            return ts.TotalHours;
        }
        /// <summary>
        /// 将时间附加到日期上面
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime AttachTime(DateTime dt, int hours, int minutes, int seconds)
        {
            DateTime time = new DateTime(dt.Year, dt.Month, dt.Day, hours, minutes, seconds);
            return time;
        }


        /// <summary>
        /// date1是否在date2之前
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsBefore(this DateTime date1, DateTime date2)
        {
            return DateTime.Compare(date2, date1) >= 0;
        }
        public static bool IsAfter(this DateTime date1, DateTime date2)
        {
            return DateTime.Compare(date1, date2) >= 0;
        }
        /// <summary>
        /// date1是否在date2和date3之间
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="date3"></param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime date1, DateTime date2, DateTime date3)
        {
            return IsBefore(date2, date1) && IsBefore(date1, date3);
        }
       
        /// <summary>
        /// 是否为同一天
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsSameDay(this DateTime date1, DateTime date2)
        {
            return date1.Date.Equals(date2.Date);
        }
        public static bool IsSameMonth(this DateTime date1, DateTime date2)
        {
            return date1.Month.Equals(date2.Month) && date1.Year.Equals(date2.Year);
        }
        public static bool IsSameWeek(this DateTime date1, DateTime date2)
        {
            return date1.GetFirstdayOfWeek().IsSameDay(date2.GetFirstdayOfWeek());
        }
        public static bool IsWeekEnd(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
        
        #endregion
    }
    
}
