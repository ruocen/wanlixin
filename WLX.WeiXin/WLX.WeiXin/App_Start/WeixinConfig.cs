﻿using Deepleo.Weixin.SDK.Helpers;

namespace WLX.WeiXin
{
    public class WeixinConfig
    {
        public static string Token { private set; get; }
        public static string EncodingAESKey { private set; get; }
        public static string AppID { private set; get; }
        public static string AppSecret { private set; get; }
        public static string PartnerKey { private set; get; }
        public static string mch_id { private set; get; }
        public static string device_info { private set; get; }
        public static string spbill_create_ip { private set; get; }
        public static string OauthScope { private set; get; }

        public static TokenHelper TokenHelper { private set; get; }


        public static void Register()
        {

            Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
            EncodingAESKey = System.Configuration.ConfigurationManager.AppSettings["EncodingAESKey"];
            AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
            AppSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
            PartnerKey = System.Configuration.ConfigurationManager.AppSettings["PartnerKey"];
            mch_id = System.Configuration.ConfigurationManager.AppSettings["mch_id"];
            device_info = System.Configuration.ConfigurationManager.AppSettings["device_info"];
            spbill_create_ip = System.Configuration.ConfigurationManager.AppSettings["spbill_create_ip"];
            var openJSSDK = int.Parse(System.Configuration.ConfigurationManager.AppSettings["OpenJSSDK"]) > 0;
            OauthScope = System.Configuration.ConfigurationManager.AppSettings["OauthScope"];
            TokenHelper = new TokenHelper(6000, AppID, AppSecret, openJSSDK);
            TokenHelper.Run();
        }
    }
}