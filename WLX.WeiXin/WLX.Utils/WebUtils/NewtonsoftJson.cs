using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WLX.Utils.WebUtils
{
   public class NewtonsoftJson
    {
       public NewtonsoftJson() { }
       /// <summary>
       /// 序列化Json
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <returns></returns>
       public static string SerializeObject(object jsonObject)
       {
           var dateTime = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };
           return JsonConvert.SerializeObject(jsonObject, Formatting.Indented, dateTime);
       }


       /// <summary>
       /// Json反系列化对象
       /// </summary>
       /// <param name="jsonObject">反系列化对象</param>
       /// <returns></returns>
       public static object DeserializeObject(string jsonObject)
       {
           return JsonConvert.DeserializeObject(jsonObject);
       }
       /// <summary>
       /// 序列化Json
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <param name="time">时间类型格式</param>
       /// <returns></returns>
       public static string SerializeObject(object jsonObject, string time)
       {
           var dateTime = new IsoDateTimeConverter { DateTimeFormat = time };
           return JsonConvert.SerializeObject(jsonObject, Formatting.Indented, dateTime);
       }

       /// <summary>
       /// 系列化Json分页
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <param name="count">对象总数</param>
       /// <returns></returns>
       public static string SerializeObjectGrid(object jsonObject, int count = 0)
       {
           string result = SerializeObject(jsonObject);
           return @"{""rows"":" + result + @",""total"":""" + count + @"""}";
       }
       /// <summary>
       /// 系列化Json分页
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <param name="time">时间类型格式</param>
       /// <param name="count">对象总数</param>
       /// <returns></returns>
       public static string SerializeObjectGrid(object jsonObject, string time, int count = 0)
       {
           string result = SerializeObject(jsonObject, time);
           return @"{""rows"":" + result + @",""total"":""" + count + @"""}";
       }

       /// <summary>
       /// 系列化Json分页
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <param name="count">对象总数</param>
       /// <param name="footer">底部合计</param>
       /// <returns></returns>
       public static string SerializeObjectGrid(object jsonObject, int count = 0, string footer = "")
       {
           string result = SerializeObject(jsonObject);
           return @"{""rows"":" + result + @",""total"":""" + count + @""",""footer"":[" + footer + "]}";
       }
       /// <summary>
       /// 系列化Json分页
       /// </summary>
       /// <param name="jsonObject">序列对象</param>
       /// <param name="time">时间格式化类型</param>
       /// <param name="count">对象总数</param>
       /// <param name="footer">底部合计</param>
       /// <returns></returns>
       public static string SerializeObjectGrid(object jsonObject, string time, int count = 0, string footer = "")
       {
           string result = SerializeObject(jsonObject, time);
           return @"{""rows"":" + result + @",""total"":""" + count + @""",""footer"":[" + footer + "]}";
       }

       /// <summary>
       /// 对象反系列化实体
       /// </summary>
       /// <param name="jsonObject">系列化对象</param>
       /// <returns></returns>
       public static T DeserializeObjectEntity<T>(string jsonObject)
       {

           return (T)JsonConvert.DeserializeObject(jsonObject, typeof(T));
       }
       /// <summary>
       /// 把Json字符串转换成对应的泛型实体集合
       /// </summary>
       /// <typeparam name="T">实体(允许泛型)</typeparam>
       /// <param name="strings">要序列化的字符串</param>
       /// <returns></returns>
       public static List<T> DeserializeObjectList<T>(string strings)
       {

           return JsonConvert.DeserializeObject<List<T>>(strings);
       }


       /// <summary>
       /// 把Json字符串转换成强类型数组
       /// </summary>
       /// <param name="jsonObject">转换的对象</param>
       /// <returns></returns>
       public static JArray SerializeObjectArray(string strings)
       {
           return (JArray)JsonConvert.DeserializeObject(strings);
       }

    }
}
