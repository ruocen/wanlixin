using System;
using System.Collections;
using System.Reflection;

namespace WLX.Data.Enums
{
    //add by wuyunxing 2015-6-25
    public class PubEnum
    {
        #region 工具函数
        private static SortedList GetStatus(System.Type t)
        {
            SortedList list = new SortedList();

            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                list.Add(enumKey, enumDescription);
            }
            return list;
        }

        private static int GetValue(System.Type t, string v)
        {
            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                if (v == enumDescription)
                {
                    return enumKey;
                }
            }
            return 0;
        }

        private static string GetName(System.Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        private static string GetDescription(System.Type t, object v)
        {
            try
            {
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }
        #endregion

        #region ValidOrInValid
        public enum ValidOrInValid : int
        {
            [Description("有效")]
            Valid = 1,
            [Description("无效")]
            InValid = 0
        }
        public static SortedList GetValidOrInValid()
        {
            return GetStatus(typeof(ValidOrInValid));
        }
        public static string GetValidOrInValid(object v)
        {
            return GetDescription(typeof(ValidOrInValid), v);
        }
        #endregion

        #region RefType(邀请码类型)
        public enum RefType : int
        {
            [Description("普通辣妈")]
            Lama = 1,
            [Description("系统活动")]
            Activity = 2,
            [Description("特殊排名")]
            Rank = 3
        }
        public static SortedList GetRefType()
        {
            return GetStatus(typeof(RefType));
        }
        public static string GetRefType(object v)
        {
            return GetDescription(typeof(RefType), v);
        }
        #endregion

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        private string description;
        public string Description
        {
            get { return description; }
        }

        public DescriptionAttribute(String description)
        {
            this.description = description;
        }
    }

    #region 宝宝情况
    /// <summary>
    /// 宝宝情况
    /// </summary>
    public enum BabySituationEnum
    {
        /// <summary>备孕</summary>
        [Description("备孕")]
        备孕 = 0,
        /// <summary>已怀孕</summary>
        [Description("已怀孕")]
        已怀孕 = 1,
        /// <summary>已有宝宝</summary>
        [Description("已有宝宝")]
        已有宝宝 = 2
    }
    #endregion

    /// <summary>
    /// 扩展枚举属性
    /// </summary>
    public static class Extension
    {
        private static string getToDescription(Type type, FieldInfo info)
        {
            DescriptionAttribute descriptionAttribute = info.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;
            if (descriptionAttribute != null)
                return descriptionAttribute.Description;
            else
                return type.ToString();
        }

        #region 宝宝情况
        /// <summary>
        /// 宝宝情况
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string ToDescription(this BabySituationEnum myEnum)
        {
            Type type = typeof(BabySituationEnum);
            FieldInfo info = type.GetField(myEnum.ToString());
            return getToDescription(type, info);
        }
        #endregion

    }
}