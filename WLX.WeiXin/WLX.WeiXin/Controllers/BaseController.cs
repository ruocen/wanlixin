using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WLX.Data.Models;
using WLX.WeiXin.Models;

namespace WLX.WeiXin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public Customer CurrentCustomer
        {
            get
            {
                //if (!DataUtils.IsLocalTest)
                //{
                //  if (Session["Membership"] == null) throw new SessionTimeoutException();
                //}
                return Session["Customer"] as Customer;
            }
            set { Session["Customer"] = value; }
        }
        public h_CustomerNew Current_h_CustomerNew
        {
            get { return Session["h_CustomerNew"] as h_CustomerNew; }
            set { Session["h_CustomerNew"] = value; }
        }
    }
}