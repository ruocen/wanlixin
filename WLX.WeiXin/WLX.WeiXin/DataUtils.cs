using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WLX.Utils;

namespace WLX.WeiXin
{
    public class DataUtils
    {
        public static int PageSize
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]); }
        }
        public static string GetRedirectToActionUrl()
        {
            try
            {
                string appid = System.Configuration.ConfigurationManager.AppSettings["AppID"];
                string token = System.Configuration.ConfigurationManager.AppSettings["Token"];
                string domain = System.Configuration.ConfigurationManager.AppSettings["Domain"];

                string redirect_uri = "http://" + domain + "/weixin/SeeMember";
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}";
                url = string.Format(url, appid, HttpUtility.UrlEncode(redirect_uri), "wanlixin");

                Tools.MessBox(string.Format("时间:{0},描述：{1}", DateTime.Now, url), "//Log//Weixin//Register//GetRedirectToActionUrl//");

                return url;
            }
            catch (Exception ex)
            {
                Tools.MessBox(string.Format("时间:{0},描述：{1}", DateTime.Now, ex.Message.ToString()), "//Log//Weixin//Register//RedirectToAction//");
                return string.Empty;
            }
        }
    }
}