using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WLX.WeiXin
{
    public class PageUtil
    {
        private static string fileVersion = ConfigurationManager.AppSettings["FileVersion"];

        public static IHtmlString Import(string path)
        {
            bool isCss = path.ToLower().EndsWith("css");
            bool isJS = path.ToLower().EndsWith("js");
            //if (DataUtils.UseLocalStaticFile)
            //{
                path = path + "?v=" + fileVersion;
            //}
            //else
            //{
            //    path = staticDomain + path + "?v=" + fileVersion;
            //}
            string result = string.Empty;
            if (isCss)
            {
                result = "<link rel='stylesheet' type='text/css' href='{0}' charset='utf-8' />";
                result = string.Format(result, path);
            }
            if (isJS)
            {
                result = "<script src='{0}'></script>";
                result = string.Format(result, path);
            }
            return new HtmlString(result);
        }
    }
}