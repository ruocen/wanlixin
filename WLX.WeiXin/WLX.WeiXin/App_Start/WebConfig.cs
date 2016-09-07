using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WLX.WeiXin
{
    public class WebConfig
    {
        public static string ApiBaseUrl
        {
            get { return ConfigurationManager.AppSettings["ApiBaseUrl"]; }
        }
    }
}