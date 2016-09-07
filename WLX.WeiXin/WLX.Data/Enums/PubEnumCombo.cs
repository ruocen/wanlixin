using System;
using System.Collections.Generic;
using System.Reflection;

namespace WLX.Data.Enums
{
    public class PubEnumCombo
    {
        public class KeyValue
        {
            public KeyValue()
            {

            }
            public KeyValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #region 获取枚举下拉列表

        /// <summary>
        /// 枚举转换
        /// </summary>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        private static List<KeyValue> GenerateList(Type enumtype)
        {
            FieldInfo[] enumFields = enumtype.GetFields();    //获取字段信息对象集合
            List<KeyValue> keylist = new List<KeyValue>();
            foreach (FieldInfo field in enumFields)
            {
                if (!field.IsSpecialName)
                {
                    var keyitem = new KeyValue
                    {
                        Key = Convert.ToInt32(field.GetRawConstantValue()).ToString(),
                        Value = field.Name
                    };       //row[1] = (int)Enum.Parse(enumType, field.Name); 也可以这样

                    keylist.Add(keyitem);
                }
            }

            return keylist;
        }
        /// <summary>
        /// 内容是编码，显示是文字，必须使用 备注方式
        /// </summary>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        private static List<KeyValue> GenerateListByDesc(Type enumtype)
        {
            FieldInfo[] enumFields = enumtype.GetFields();    //获取字段信息对象集合
            List<KeyValue> keylist = new List<KeyValue>();
            foreach (FieldInfo field in enumFields)
            {
                if (!field.IsSpecialName)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    string newValue = field.Name;
                    if (attr != null) { newValue = attr.Description; }

                    var keyitem = new KeyValue
                    {
                        Key = field.Name,
                        Value = newValue

                    };
                    keylist.Add(keyitem);
                }
            }
            return keylist;
        }

        #endregion




        #region 订单状态
        /// <summary>
        /// 订单状态 
        /// </summary>
        /// <returns></returns>
        public static List<KeyValue> GetBabySituation()
        {
            return GenerateList(typeof(BabySituationEnum));

        }
        #endregion
    }
}
