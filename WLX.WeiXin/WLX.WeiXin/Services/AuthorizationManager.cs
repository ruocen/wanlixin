using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WLX.WeiXin.Services
{
    public class AuthorizationManager
    {
        public static void SetTicket(bool remeberMe, int version, string identity, string displayName)
        {
            FormsAuthentication.SetAuthCookie(identity, remeberMe);
            if (string.IsNullOrEmpty(displayName))//displayName为空会导致cookies写入失败
            {
                displayName = "匿名";
            }
            var authTicket = new FormsAuthenticationTicket(
                version,//版本号
                identity,//与身份验证票关联的用户名
                DateTime.Now, // Cookie 的发出时间
                DateTime.Now.AddDays(remeberMe ? 30 : 1),// Cookie 的到期日期
                remeberMe, //如果 Cookie 是持久的，为 true；否则为 false
                displayName);
            string encrytedTicket = FormsAuthentication.Encrypt(authTicket);//加密
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrytedTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}