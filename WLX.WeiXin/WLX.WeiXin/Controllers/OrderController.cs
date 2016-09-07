using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WLX.WeiXin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            ViewBag.c_Identity = Current_h_CustomerNew.c_Identity;
            return View();
        }
        public ActionResult OrderDetail(string orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}