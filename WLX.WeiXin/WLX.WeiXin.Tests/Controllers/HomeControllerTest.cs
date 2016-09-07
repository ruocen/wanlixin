using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WLX.WeiXin;
using WLX.WeiXin.Controllers;
using WLX.DAL;
using WLX.Data.Models;
using WLX.Utils;
using WLX.WeiXin.Models;
using WLX.Data;

namespace WLX.WeiXin.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test1()
        {
            LoginService _loginservice = new LoginService();
            Customer customer = _loginservice.CustomerLogin("兴辉", "o3MJTwmO0wm9apY1tWmQMb4CgIzc", "", "", @"http:\/\/wx.qlogo.cn\/mmopen\/GqIlejFTbNiaq6VwicYD4oGRqjPVo3XKWxvsrj4u0OMeHvL4eVPJ9sX0T48ZvYA8UUK3sDX7suqUjXBTu3G8guvw\/0", 1, "福建", "厦门");
        }
        [TestMethod]
        public void Test2()
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("vipNum", "o1MJTwpfDZ50QX3ZByUvpdPUoIeM");
            //"[]"
            var usermodel = ApiHelper.ApiByGet("http://114.55.66.247:7777/api/WlxApi/GetCustomerNewBySql", parameters);
        }
        [TestMethod]
        public void Test3()
        {
            //更新openid,nickname等会员微信信息到用户表(通过接口)
            updateCustomer updatcustomer = new updateCustomer();
            updatcustomer.openid = "o1MJTwqmd2HhAP3Wwmayc84H-Wx4";
            updatcustomer.wx_name = "笑看人生";
            updatcustomer.vpid = "5002739";

            updatcustomer.name = "黄紫俊";
            updatcustomer.sex = "男";
            updatcustomer.address = "厦门";
            updatcustomer.birthday = DateTime.Now;
            updatcustomer.num = "15960290707";

            var result = WebUtils.Save<updateCustomer>("http://114.55.66.247:7777/api/WlxApi/UpdateVip", updatcustomer);
        }
        [TestMethod]
        public void Test4()
        {
            var usermodel = WebUtils.Get2<CustomerNewSelect>("http://114.55.66.247:7777/api/WlxApi/GetCustomerNewBySql", "o1MJTwpfDZ50QX3ZByUvpdPUoIeM");
        }
        [TestMethod]
        public void Test5()
        {
            string mobile = "13062449866";
            InsertCustomerNew customer = new InsertCustomerNew();

            //根据mobile获取openid和wxname信息；
            var model = new Customer();
            //using (EntityContext db = new EntityContext())
            //{
            //    model = db.Customers.Where(s => s.Phone == mobile).FirstOrDefault();
            //}
            customer.num = mobile;
            customer.openid = "o1MJTwmO0wm9apY1tWmQMb4CgIzc";
            customer.wxname = "兴辉";

            //var inserResult = WebUtils.Save<InsertCustomerNew>(WebConfig.ApiBaseUrl + "WlxApi/InsertVip", customer);
            var result = WebUtils.Save<InsertCustomerNew>("http://114.55.66.247:7777/api/WlxApi/InsertVip", customer);
        }

    }
}
