using System;
using System.ComponentModel;
using System.Reflection;

namespace WLX.Utils.WebUtils
{
    public static class EnumHelper
    {
        public static void GetEnumDesc<Ttype>(Ttype Enumtype)
        {
            if (Enumtype == null) throw new ArgumentNullException("Enumtype");
            if (!Enumtype.GetType().IsEnum) throw new Exception("参数类型不正确");

            FieldInfo[] fieldinfo = Enumtype.GetType().GetFields();
            foreach (FieldInfo item in fieldinfo)
            {
                Object[] obj = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (obj != null && obj.Length != 0)
                {
                    DescriptionAttribute des = (DescriptionAttribute)obj[0];
                    Console.WriteLine(des.Description);
                }
            }
        }

        /// <summary>
        /// only used for enum convert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (int item in Enum.GetValues(typeof(T)))
            {
                string strName = Enum.GetName(typeof(T), item);//获取名称
                if (strName == value)
                {
                    return (T)Enum.Parse(typeof(T), strName);
                }
            }

            throw new ArgumentException("转换失败", "value");
        }

        /// <summary>
        /// only used for enum convert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this int value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (int item in Enum.GetValues(typeof(T)))
            {
                if (item == value)
                {
                    string strName = Enum.GetName(typeof(T), item);//获取名称
                    return (T)Enum.Parse(typeof(T), strName);
                }
            }

            throw new ArgumentException("转换失败", "value");
        }
    }
}
