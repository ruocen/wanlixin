using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WLX.Utils
{
    public class ApiHelper
    {
        public static int URL_TIMEOUT = 30000;
        public static string ApiByGet(string url, IDictionary<string, object> parameters)
        {
            HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponseByParams(url, parameters, URL_TIMEOUT, "", null);
            var sr = new System.IO.StreamReader(response.GetResponseStream());
            string content = sr.ReadToEnd(); //这里的content就是网页内容了 
            sr.Close();
            System.Diagnostics.Debug.WriteLine(content);
            return content;
        }
        public static string ApiByPost(string url, IDictionary<string, object> parameters)
        {
            HttpWebResponse response = HttpWebResponseUtility.CreateApiPostHttpResponse(url, parameters, URL_TIMEOUT, "", Encoding.UTF8, null);
            var sr = new System.IO.StreamReader(response.GetResponseStream());
            string content = sr.ReadToEnd(); //这里的content就是网页内容了 
            sr.Close();
            System.Diagnostics.Debug.WriteLine(content);
            return content;
        }

        public static T GetApiData<T>(ApiBody body, ref int totalCount)
        {
            string returns = string.Empty;
            //参数
            if (body.SendObj != null)
                body.Params = GetParameters(body.SendObj);
            if (body.SendType == SendType.Get)
            {
                returns = ApiByGet(body.ApiUrl, body.Params);
                Tools.MessBox("GetApiData:" + returns);
            }
            else if (body.SendType == SendType.Post)
            {
                returns = ApiByPost(body.ApiUrl, body.Params);
            }
            if (!string.IsNullOrEmpty(returns))
            {
                try
                {
                    var t = JsonConvert.DeserializeObject<T>(returns);
                    return t;
                }
                catch (Exception ex)
                {
                    Tools.MessBox("GetApiData:" + ex.ToString());
                    return default(T);
                }
            }
            return default(T);
        }
        public static bool GetApiData(ApiBody body, ref int totalCount, ref string msg)
        {
            string returns = string.Empty;
            //参数
            if (body.SendObj != null)
                body.Params = GetParameters(body.SendObj);
            if (body.SendType == SendType.Get)
            {
                returns = ApiByGet(body.ApiUrl, body.Params);
            }
            else if (body.SendType == SendType.Post)
            {
                //var paramsa = NewGetPrarams(body.SendObj);
                returns = ApiByPost(body.ApiUrl, body.Params);
            }
            var result = false;
            if (!string.IsNullOrEmpty(returns))
            {
                var s = Newtonsoft.Json.JsonConvert.DeserializeObject(returns);
                var obj = JObject.Parse(s.ToString());
                result = Convert.ToBoolean(obj["IsSuccessful"]);
                if (result && obj["Data"].ToString() != "[]")
                {
                    totalCount = obj["TotalCount"] == null ? 0 : Convert.ToInt32(obj["TotalCount"]);
                }
                msg = obj["ErrorMessage"].ToString();
            }
            return result;
        }
        public static bool GetApiData(ApiBody body, ref int totalCount)
        {
            string returns = string.Empty;
            //参数
            if (body.SendObj != null)
                body.Params = GetParameters(body.SendObj);
            if (body.SendType == SendType.Get)
            {
                returns = ApiByGet(body.ApiUrl, body.Params);
            }
            else if (body.SendType == SendType.Post)
            {
                //var paramsa = NewGetPrarams(body.SendObj);
                returns = ApiByPost(body.ApiUrl, body.Params);
            }
            var result = false;
            if (!string.IsNullOrEmpty(returns))
            {
                var s = Newtonsoft.Json.JsonConvert.DeserializeObject(returns);
                //var obj = JObject.Parse(s.ToString());
                result = Convert.ToBoolean(s);
                //if (result && obj["Data"].ToString() != "[]")
                //{
                //    totalCount = obj["TotalCount"] == null ? 0 : Convert.ToInt32(obj["TotalCount"]);
                //}
            }
            return result;
        }
        public static IDictionary<string, object> GetParameters(object obj)
        {
            IDictionary<string, object> Params = new Dictionary<string, object>();
            if (obj != null)
            {
                var type = obj.GetType();
                PropertyInfo[] properties = type.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToArray();

                foreach (PropertyInfo info in properties)
                {
                    try
                    {
                        Params.Add(info.Name, GetObjectPropertyValue(obj, info.Name));
                    }
                    catch (Exception ex) { }

                }
            }
            return Params;
        }
        public static IDictionary<string, string> NewGetPrarams(object obj)
        {
            IDictionary<string, string> Params = new Dictionary<string, string>();
            if (obj != null)
            {
                var type = obj.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo info in properties)
                {
                    try
                    {
                        Params.Add(info.Name, GetObjectPropertyValue(obj, info.Name));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                }
            }
            return Params;
        }
        public static string GetObjectPropertyValue(object obj, string propertyname)
        {
            var t = obj.GetType();
            PropertyInfo property = t.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(obj, null);


            if (o == null) return string.Empty;

            return o.ToString();
        }
    }


    public class ApiBody
    {
        public string ApiUrl { get; set; }
        public IDictionary<string, object> Params { get; set; }
        public SendType SendType { get; set; }
        public Object SendObj { get; set; }
    }
    public enum SendType
    {
        Get,
        Post,
        Put,
        Delete
    }
    public class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponseByParams(string url, IDictionary<string, object> parameters, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        url += string.Format("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        url += string.Format("?{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateApiPostHttpResponse(string url, IDictionary<string, object> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";



            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static async Task<string> DoPostTask(string url, IDictionary<string, string> paramsa)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                var content = new FormUrlEncodedContent(paramsa);
                var response = await http.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }

        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
