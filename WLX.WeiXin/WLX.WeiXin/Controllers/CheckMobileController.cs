using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WLX.DAL;
using WLX.DAL.Models;
using WLX.Data.Enums;
using WLX.Data.Models;

namespace WLX.WeiXin.Controllers
{
    public class CheckMobileController : Controller
    {
        // GET: CheckMobile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendCheckCode(string mobile)
        {
            CheckMobileStatus status = CheckMobileStatus.错误;

            string msg = "";

            if (WLX.Utils.Validator.IsMobile(mobile))
            {
                UserCheckMobile checkinfo = new UserCheckMobile();
                checkinfo.Mobile = mobile;
                checkinfo.CheckCode = WLX.Utils.Utils2.GetRandomString(1, 6, "0123456789").ToUpper();
                //checkinfo.UserID = onlineUserInfo.UserID;
                checkinfo.GetCodeTimes = 1;

                //发送短信验证码
                 status = SMSService.SendMobileCheckCode(checkinfo);

                switch (status)
                {
                    case CheckMobileStatus.成功发送验证码:
                        msg = "已发送验证码，请注意查收。";
                        break;
                    case CheckMobileStatus.这个手机已经绑定:
                        msg = "您已绑定手机，如需更改请输入新手机号码。";
                        break;
                    case CheckMobileStatus.验证间隔时间太短:
                        msg = "一分钟只能发送一次验证码，请稍后再试。";
                        break;
                    case CheckMobileStatus.手机已被其他账户绑定:
                        msg = "您填写的手机号码已绑定其他账户，如有疑问请咨询客服，谢谢。";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                msg = "手机号码有误，请输入正确的手机号码。";
            }

            if (status == CheckMobileStatus.成功发送验证码)
            {
                return Content("已发送验证码至：" + mobile, "checkmobile.aspx?action=step2&mobile=" + mobile);
            }
            else
            {
                return Content(msg);
            }
        }
    }
}