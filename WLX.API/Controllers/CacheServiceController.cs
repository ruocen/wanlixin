using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WLX.API.Controllers;
using WLX.API.Models;
using WLX.Data.DataBase;
using WLX.Utils;
using WLX.Utils.WebUtils;

namespace WLX.API.Controllers
{
    public class CacheServiceController : ApiController
    {
        public static void ThreadCache()
        {
            //线程
            Thread threadSendOrderNotice = new Thread(new ThreadStart(ThreadSendOrderNotice));
            Tools.MessBox(DateTime.Now + "定时推送订单消费信息 开始执行", "//LOG//CacheService//");
            threadSendOrderNotice.Start();
        }
        private static void ThreadSendOrderNotice()
        {
            WlxApiController api = new WlxApiController();
            //从数据库中检索未发送订单消息的订单orderList(最近)
            

            List<Order> orderList = new List<Order>();
            string sql = @"select c_chdh as orderId from h_chtj where c_wxts='1' and c_vipid>0";
            DataSet ds = SqlHelper.ExecuteDataSet(CommandType.Text, sql);
            orderList = EntityObject.DT2EntityList<Order>(ds.Tables[0]);

            foreach (var item in orderList)
            {
                api.sendOrder(item.orderId);
            }          
            Thread.Sleep(60 * 1000);//60s跑一次
        }
    }
}
