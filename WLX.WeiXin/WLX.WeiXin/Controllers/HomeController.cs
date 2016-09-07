using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WLX.DAL;
using WLX.Data;
using WLX.Data.Enums;
using WLX.Data.Filter;
using WLX.Data.Models;
using WLX.Utils;
using WLX.Utils.WebUtils;
using WLX.WeiXin.Models;
using static WLX.Utils.JsonHandler;

namespace WLX.WeiXin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Member()
        {
            Tools.MessBox("test");
            ViewBag.vipNum = CurrentCustomer.Phone;
            Tools.MessBox("Home/Member:" + CurrentCustomer.Phone);
            return View();
        }
        /// <summary>
        /// 积分明细
        /// </summary>
        public ActionResult IntegralList()
        {
            ViewBag.c_Identity = Current_h_CustomerNew.c_Identity;
            return View();
        }
        public ActionResult UpdateUser(string keyword, int? page)
        {
            int pageIndex = page ?? 1;
            var filter = new KeywordPageFilter(keyword, pageIndex, DataUtils.PageSize);
            var list = ""; //WebUtils.PageList<Customer>(WebConfig.ApiBaseUrl + "Foundation/Schedule/Paged", filter);
            return View(list);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Register(string mobile,string code)
        {
            try
            {
                //判断验证码是否正确
                UserCheckMobile existCheckMobileUserInfo = CheckMobileService.GetLastCheckMobileInfo(mobile);
                if (code != existCheckMobileUserInfo.CheckCode)
                {
                    return Json(JsonHandler.CreateMessage(0, "验证码错误，请重新输入!"));
                }

                //根据手机号码取出会员信息
                var usermodel = WebUtils.Get<CustomerNew>(WebConfig.ApiBaseUrl + "WlxApi/GetCustomerBySql", mobile);
                //把openid等信息更新到万里信erp
                if (usermodel == null)
                {
                    Tools.MessBox("test1");
                    InsertCustomerNew customer = new InsertCustomerNew();
                    
                    customer.num = mobile;
                    customer.openid = CurrentCustomer.OpenId;
                    customer.wxname = CurrentCustomer.Name;

                    var inserResult = WebUtils.Save<InsertCustomerNew>(WebConfig.ApiBaseUrl + "WlxApi/InsertVip", customer);
                    Tools.MessBox("test2");
                    var url = DataUtils.GetRedirectToActionUrl();
                    return Json(JsonHandler.CreateMessage(1, "注册成功", url), JsonRequestBehavior.AllowGet);
                    //return Json(JsonHandler.CreateMessage(0, "您不是线下会员,请到附近门店办理会员后再来关联"), JsonRequestBehavior.AllowGet);
                }
                updateCustomer updatcustomer = new updateCustomer();
                updatcustomer.openid = CurrentCustomer.OpenId;
                updatcustomer.wx_name = CurrentCustomer.Name;
                updatcustomer.vpid = usermodel.c_identity.ToString();

                var result = WebUtils.Save<updateCustomer>(WebConfig.ApiBaseUrl + "WlxApi/UpdateVip", updatcustomer);

                if (result)
                {
                    Customer customer = new Customer();
                    customer.Phone = mobile;
                    Session["Customer"] = customer;
                    Tools.MessBox(string.Format("时间:{0},描述：{1}", DateTime.Now, customer.Phone), "//Log//Register//Session//");
                    var url = DataUtils.GetRedirectToActionUrl();
                    return Json(JsonHandler.CreateMessage(1, "注册成功", url), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "注册失败"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString(), "//Log//Register//Exception");
                return Json(JsonHandler.CreateMessage(0, "注册失败", Url.Action("Index")), JsonRequestBehavior.AllowGet);
            }
          
        }

    }
}