using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WLX.Utils.WebUtils
{
    public static class JsonUtils
    {
        public const string STATUS = "status";
        public const string STATUS_TRUE = "true";
        public const string STATUS_FALSE = "false";

        public const string MSG = "msg";
        public const string MSG_OK = "ok";
        public const string MSG_ERROR = "error";

        public const string TOTAL = "totalcount";

        public static ContentResult GetJsonResult(string jsonStr)
        {
            StringBuilder buider = new StringBuilder("{");
            buider.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            buider.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
            buider.AppendFormat("{0}", jsonStr.Substring(1, jsonStr.Length - 2));
            buider.Append('}');

            return ContentResult(buider);
        }

        public static ContentResult GetJsonResult(string msg, string jsonStr)
        {
            StringBuilder buider = new StringBuilder("{");
            buider.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            buider.AppendFormat("\"{0}\":\"{1}\",", MSG, msg);
            buider.AppendFormat("{0}", jsonStr.Substring(1, jsonStr.Length - 2));
            buider.Append('}');

            return ContentResult(buider);
        }

        public static ContentResult GetJsonResult(object obj)
        {
            return GetJsonResult(obj, null);
        }
        public static ContentResult GetJsonResult(object obj, int? totalCount)
        {
            StringBuilder builder = new StringBuilder("{");
            if (obj is IEnumerable)
            {

                var types = obj.GetType().GetGenericArguments();

                var type = types.Length > 0 ? types[0] : typeof(object); //获取泛型类型

                //[{"id":1},{"id":2}]
                string json = JsonConvert.SerializeObject(obj);

                builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
                builder.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
                if (totalCount == null)
                {
                    builder.AppendFormat("\"{0}\":\"\",", TOTAL);
                }
                else
                {
                    builder.AppendFormat("\"{0}\":{1},", TOTAL, totalCount);
                }
                builder.AppendFormat("\"{0}\":{1}", type.Name.ToLower(), json);
                builder.Append("}");

                return ContentResult(builder);

            }
            else
            {
                string name = obj.GetType().Name.ToLower();
                string objJson = JsonConvert.SerializeObject(obj);

                builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
                builder.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
                builder.AppendFormat("\"{0}\":{1}", name, objJson);
                builder.Append("}");

                return ContentResult(builder);
            }

        }

        public static ContentResult GetJsonResult<T>(T obj)
        {
            string name = obj.GetType().Name.ToLower();

            string objJson = JsonConvert.SerializeObject(obj);

            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            builder.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
            builder.AppendFormat("\"{0}\":{1}", name, objJson);
            builder.Append("}");

            return ContentResult(builder);
        }

        public static ContentResult GetJsonResult<T>(IEnumerable<T> list) where T : new()
        {
            T t = new T();
            string root = string.Format("{0}{1}", t.GetType().Name.ToLower(), "s");

            return GetJsonResult<T>(list, root);
        }

        public static ContentResult GetJsonResult<T>(IEnumerable<T> list, int totalCount) where T : new()
        {
            T t = new T();
            string root = string.Format("{0}{1}", t.GetType().Name.ToLower(), "s");

            return GetJsonResult<T>(list, root, totalCount);
        }

        public static ContentResult GetJsonResult<T>(IEnumerable<T> list, string root) where T : new()
        {
            return GetJsonResult<T>(list, root, null);
        }

        public static ContentResult GetJsonResult<T>(IEnumerable<T> list, string root, int? totalCount) where T : new()
        {
            StringBuilder builder = new StringBuilder("{");

            //[{"id":1},{"id":2}]
            string json = JsonConvert.SerializeObject(list);

            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            builder.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
            if (totalCount == null)
            {
                builder.AppendFormat("\"{0}\":\"\",", TOTAL);
            }
            else
            {
                builder.AppendFormat("\"{0}\":{1},", TOTAL, totalCount);
            }
            builder.AppendFormat("\"{0}\":{1}", root, json);
            builder.Append("}");

            return ContentResult(builder);
        }

        /// <summary>
        /// 作者：王赟
        /// 时间：2014-12-18
        /// 描述：重载GetJsonResult方法，添加时间格式参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="root"></param>
        /// <param name="time">
        /// </param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static ContentResult GetJsonResult<T>(IEnumerable<T> list, string root, string time, int? totalCount) where T : new()
        {
            StringBuilder builder = new StringBuilder("{");

            //[{"id":1},{"id":2}]
            var dateTime = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = time };
            string json = JsonConvert.SerializeObject(list, Formatting.Indented, dateTime);

            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            builder.AppendFormat("\"{0}\":\"{1}\",", MSG, MSG_OK);
            if (totalCount == null)
            {
                builder.AppendFormat("\"{0}\":\"\",", TOTAL);
            }
            else
            {
                builder.AppendFormat("\"{0}\":{1},", TOTAL, totalCount);
            }
            builder.AppendFormat("\"{0}\":{1}", root, json);
            builder.Append("}");

            return ContentResult(builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fun"></param>
        /// <returns>json string</returns>
        public static ContentResult GetJsonResult(Func<string> func)
        {
            return SafeContentResult(() =>
            {
                return GetJsonResult(func());
            });
        }

        public static ContentResult GetJsonResult(Func<object> func)
        {
            return SafeContentResult(() =>
            {
                return GetJsonResult(func());
            });
        }

        public static ContentResult GetJsonResult<T>(Func<IEnumerable<T>> func) where T : new()
        {
            return SafeContentResult(() =>
            {
                return GetJsonResult<T>(func());
            });
        }

        /// <summary>
        /// Tuple<IEnumerable<T>,int>
        /// 第一个参数是列表集合，第二个参数是所有记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static ContentResult GetJsonResult<T>(Func<Tuple<IEnumerable<T>, int>> func) where T : new()
        {
            return SafeContentResult(() =>
            {
                var tuple = func();

                return GetJsonResult<T>(tuple.Item1, tuple.Item2);
            });
        }

        /// <summary>
        /// Tuple<IEnumerable<T>,string>
        /// 第一个参数是列表集合，第二个参数root
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static ContentResult GetJsonResult<T>(Func<Tuple<IEnumerable<T>, string>> func) where T : new()
        {
            return SafeContentResult(() =>
            {
                var tuple = func();

                return GetJsonResult<T>(tuple.Item1, tuple.Item2);
            });
        }

        public static ContentResult GetJsonResult<T>(Func<Tuple<IEnumerable<T>, string, int>> func) where T : new()
        {
            return SafeContentResult(() =>
            {
                var tuple = func();

                return GetJsonResult<T>(tuple.Item1, tuple.Item2, tuple.Item3);
            });
        }


        public static ContentResult SafeContentResult(Func<ContentResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        private static ContentResult ContentResult(StringBuilder sb)
        {
            return new ContentResult
            {
                Content = sb.ToString(),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        public static ContentResult ErrorResult(Exception ex)
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_FALSE);
            builder.AppendFormat("\"{0}\":\"{1}\"", MSG, ex.Message);
            builder.Append("}");

            return new ContentResult
            {
                Content = builder.ToString(),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        public static ContentResult SuccessResult()
        {
            return SuccessResult("");
        }

        public static ContentResult SuccessResult(string msg)
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_TRUE);
            builder.AppendFormat("\"{0}\":\"{1}\"", MSG, msg);
            builder.Append("}");

            return new ContentResult
            {
                Content = builder.ToString(),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        public static ContentResult ErrorResult()
        {
            return ErrorResult("");
        }

        public static ContentResult ErrorResult(string msg)
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"{0}\":\"{1}\",", STATUS, STATUS_FALSE);
            builder.AppendFormat("\"{0}\":\"{1}\"", MSG, msg);
            builder.Append("}");

            return new ContentResult
            {
                Content = builder.ToString(),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        public static ContentResult BoolResult(bool result)
        {
            return result ? SuccessResult() : ErrorResult();
        }

        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }
    }
}
