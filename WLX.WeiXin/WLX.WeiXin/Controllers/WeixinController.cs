using Deepleo.Weixin.SDK;
using Senparc.Weixin.MP.Containers;
using System;
using System.IO;
using System.Web.Mvc;
using Tencent;
using WLX.DAL;
using WLX.Data.Models;
using WLX.Utils;
using WLX.WeiXin.Models;
using WLX.WeiXin.Services;

namespace WLX.WeiXin.Controllers
{
    public class WeixinController : BaseController
    {
        // GET: Weixin
        public WeixinController()
        {

        }
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            var token = WeixinConfig.Token;//微信公众平台后台设置的Token
            if (string.IsNullOrEmpty(token)) return Content("请先设置Token！");
            var ent = "";
            if (!BasicAPI.CheckSignature(signature, timestamp, nonce, token, out ent))
            {
                return Content("参数错误！");
            }
            return Content(echostr); //返回随机字符串则表示验证通过
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(string signature, string timestamp, string nonce, string echostr)
        {
            WeixinMessage message = null;
            var safeMode = Request.QueryString.Get("encrypt_type") == "aes";
            using (var streamReader = new StreamReader(Request.InputStream))
            {
                var decryptMsg = string.Empty;
                var msg = streamReader.ReadToEnd();

                #region 解密
                if (safeMode)
                {
                    var msg_signature = Request.QueryString.Get("msg_signature");
                    var wxBizMsgCrypt = new WXBizMsgCrypt(WeixinConfig.Token, WeixinConfig.EncodingAESKey, WeixinConfig.AppID);
                    var ret = wxBizMsgCrypt.DecryptMsg(msg_signature, timestamp, nonce, msg, ref decryptMsg);
                    if (ret != 0)//解密失败
                    {
                        //TODO：开发者解密失败的业务处理逻辑
                        //注意：本demo用log4net记录此信息，你可以用其他方法
                        LogWriter.Default.WriteError(string.Format("decrypt message return {0}, request body {1}", ret, msg));
                    }
                }
                else
                {
                    decryptMsg = msg;
                }
                #endregion

                message = AcceptMessageAPI.Parse(decryptMsg);
            }
            var response = new WeixinExecutor().Execute(message);
            var encryptMsg = string.Empty;

            #region 加密
            if (safeMode)
            {
                var msg_signature = Request.QueryString.Get("msg_signature");
                var wxBizMsgCrypt = new WXBizMsgCrypt(WeixinConfig.Token, WeixinConfig.EncodingAESKey, WeixinConfig.AppID);
                var ret = wxBizMsgCrypt.EncryptMsg(response, timestamp, nonce, ref encryptMsg);
                if (ret != 0)//加密失败
                {
                    //TODO：开发者加密失败的业务处理逻辑
                    LogWriter.Default.WriteError(string.Format("encrypt message return {0}, response body {1}", ret, response));
                }
            }
            else
            {
                encryptMsg = response;
            }
            #endregion

            return new ContentResult
            {
                Content = encryptMsg,
                ContentType = "text/xml",
                ContentEncoding = System.Text.UTF8Encoding.UTF8
            };
        }


        /// <summary>
        /// 会员中心
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DefaultAuthorize(string code, string state)
        {
            try
            {
                if (state == "wanlixin")
                {
                    var scope = WeixinConfig.OauthScope;
                    double expires_in = 0;
                    var access_token = "";
                    var openId = "";
                    var token = OAuth2API.GetAccessToken(code, WeixinConfig.AppID, WeixinConfig.AppSecret);
                    dynamic userinfo;

                    //access_token = WeixinConfig.TokenHelper.GetToken();//基础支持中的access_token
                    access_token = AccessTokenContainer.TryGetAccessToken(WeixinConfig.AppID, WeixinConfig.AppSecret, true);

                    openId = token.openid;
                    expires_in = token.expires_in;

                    //TODO: 如果用户已经关注，可以用openid，获取用户信息。
                    userinfo = UserAdminAPI.GetInfo(access_token, openId);//如果本地已经存储了用户基本信息，建议在本地获取。

                    Tools.MessBox(string.Format("时间:{0},描述：{1}", DateTime.Now, userinfo), "//Log//Weixin//DefaultAuthorize//userinfo//");

                    //通过api接口，根据openid查找，如果openid已经保存,跳转到”会员积分查询“页面，否则跳转到”输入手机号码页面“

                    Tools.MessBox("openId:"+ openId);

                    var usermodel = WebUtils.Get2<CustomerNewSelect>(WebConfig.ApiBaseUrl + "WlxApi/GetCustomerNewBySql", openId);

                    //Tools.MessBox("usermodel:"+ usermodel.c_num);
                    if (usermodel!=null)
                    {
                        if (!string.IsNullOrEmpty(usermodel.c_num))//c_openid已经更新过
                        {
                            Customer customer = new Customer();
                            customer.Phone = usermodel.c_num;
                            Session["Customer"] = customer;

                            h_CustomerNew h_CustomerNew = new h_CustomerNew();
                            h_CustomerNew.c_num = usermodel.c_num;
                            h_CustomerNew.c_Name = usermodel.c_Name;
                            h_CustomerNew.c_Identity = usermodel.c_Identity;
                            Session["h_CustomerNew"] = h_CustomerNew;

                            return RedirectToAction("Member", "Home");
                        }                     
                    }
                    else
                    {
                        //插入微信信息
                        LoginService _loginservice = new LoginService();
                        Customer customer = _loginservice.CustomerLogin(userinfo.nickname, userinfo.openid, "", "", userinfo.headimgurl, Convert.ToInt32(userinfo.sex), userinfo.province, userinfo.city);
                        if (customer != null)
                        {
                            Session["Customer"] = customer;
                        }
                        return RedirectToAction("Index", "Home");
                    }

                }

                return Content("验证失败4");
            }
            catch (Exception ex)
            {
                string st = string.Format("时间:{0},描述：{1}", DateTime.Now, ex.ToString());
                Tools.MessBox(st, "//Log//Weixin//");
                return Redirect("/error/index");
            }
        }

        [HttpGet]
        public ActionResult Login(string code, string state)
        {
            try
            {
                if (state == "wanlixin")
                {
                    return RedirectToAction("Member", "Home");
                }

                return Content("登录验证失败4");
            }
            catch (Exception ex)
            {
                string st = string.Format("时间:{0},描述：{1}", DateTime.Now, ex.ToString());
                Tools.MessBox(st, "//Log//Weixin//");
                return Redirect("/error/index");
            }
        }

        #region 推送消息查看订单详情
        [HttpGet]
        public ActionResult SeeOrderDetail(string orderid, string code, string state)
        {
            try
            {
                //CurrentCustomer = null;
                if (state == "wanlixin")
                {
                    var token = OAuth2API.GetAccessToken(code, WeixinConfig.AppID, WeixinConfig.AppSecret);
                    string openId = token.openid;
                    return Redirect("/Order/OrderDetail?orderId=" + orderid + " ");

                }

                return Content("验证失败4");
            }
            catch (Exception ex)
            {
                string st = string.Format("时间:{0},描述：{1}", DateTime.Now, ex.ToString());
                Tools.MessBox(st, "//Log//Weixin//OrderDetail");
                return Redirect("/error/index");
            }
        }
        #endregion

        public ActionResult SeeMember(string code, string state)
        {
            try
            {
                if (state == "wanlixin")
                {
                    var token = OAuth2API.GetAccessToken(code, WeixinConfig.AppID, WeixinConfig.AppSecret);
                    var usermodel = WebUtils.Get2<CustomerNewSelect>(WebConfig.ApiBaseUrl + "WlxApi/GetCustomerNewBySql", token.openid);
                    if (!string.IsNullOrEmpty(usermodel.c_num))//c_openid已经更新过
                    {
                        Customer customer = new Customer();
                        customer.Phone = usermodel.c_num;
                        Session["Customer"] = customer;

                        h_CustomerNew h_CustomerNew = new h_CustomerNew();
                        h_CustomerNew.c_num = usermodel.c_num;
                        h_CustomerNew.c_Name = usermodel.c_Name;
                        h_CustomerNew.c_Identity = usermodel.c_Identity;
                        Session["h_CustomerNew"] = h_CustomerNew;

                        return RedirectToAction("Member", "Home");
                    }
                }

                return Content("验证失败4");
            }
            catch (Exception ex)
            {
                string st = string.Format("时间:{0},描述：{1}", DateTime.Now, ex.ToString());
                Tools.MessBox(st, "//Log//Weixin//SeeMember");
                return Redirect("/error/index");
            }
        }

        /// <summary>
        /// 获取会员登录信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //private Membership GetMemberLogin(string openId, out string message)
        //{
        //    Membership mem;
        //    message = "";
        //    var access_token = "";

        //    if (string.IsNullOrEmpty(CacheHelper.GetCache("openId_" + openId)))
        //    {
        //        access_token = WeixinConfig.TokenHelper.GetToken();//基础支持中的access_token
        //        //登录成功
        //        var userinfo = OAuth2API.GetUserInfo(access_token, openId);
        //        if (string.IsNullOrEmpty(userinfo.openid))
        //        {
        //            string me = string.Format("时间：{0},描述：{1},函数:{2}", DateTime.Now, JsonConvert.SerializeObject(userinfo),
        //                "LoginStore");
        //            Tools.MessBox(me, "//LOG//Weixin//");
        //            message = "验证失败2";
        //            return null;
        //        }

        //        mem = api.LamaLogin(userinfo.nickname, userinfo.openid, "", "", userinfo.headimgurl,
        //            Convert.ToString(userinfo.sex), userinfo.province, userinfo.city, userinfo.unionid);
        //    }
        //    else
        //    {
        //        mem = api.LamaLogin("", openId, "", "", "", "", "", "", "");
        //    }
        //    CurrentMembership = mem;
        //    return mem;
        //}
    }
}