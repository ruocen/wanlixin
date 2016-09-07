using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Utils
{
    public class HttpUtils
    {
        /// <summary>
        /// 发送http请求，返回响应html
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="postData">post表单数据</param>
        /// <param name="method">http请求方式</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetWebRequest(string url, string postData, string method, Encoding encoding)
        {
            //if (!IsUrl(url)) { throw new ArgumentException("网络地址无效！", "urlHead"); }
            //if (!IsMethodOk(method)) { throw new ArgumentException("Http协议谓词无效！", "method"); }

            string html = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Method = method;

                if (method == "POST")
                {
                    byte[] data = encoding.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                        reqStream.Close();
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {

                        CookieContainer cc = new CookieContainer();
                        request.CookieContainer = cc;

                        string aaa = response.Headers.Get("Set-Cookie");

                        if (response.StatusCode == HttpStatusCode.OK && request.HaveResponse)
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                if (sr != null)
                                {
                                    html = sr.ReadToEnd();
                                    string cookie = response.Cookies.ToString();
                                }
                            }
                        }
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                html = ex.ToString();
            }
            return html;
        }
    }
}
