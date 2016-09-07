using Deepleo.Weixin.SDK;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WLX.Data;
using WLX.Utils;
using WLX.WeiXin.Models;

namespace WLX.WeiXin.Controllers
{
    [RoutePrefix("api/WlxApi")]
    public class wlxapiController : ApiController
    {
        //http://localhost:14639/api/sale/?id=5002740&name=T恤

        CommDAL dal = new CommDAL();
        #region
        /*
        /// <summary>
        /// 会员绑定
        /// </summary>
        /// <param name="wxNum"></param>
        /// <returns></returns>
        /// h_CustomerNew会员资料表
        /// c_num 会员编号
        /// c_identity 会员ID（唯一标识）
        [HttpGet, Route("GetCustomerBySql")]
        public string GetCustomerBySql(string telNum)
        {
            var sql = @"select c_num,c_identity from h_CustomerNew where 1=1";
            if (!string.IsNullOrEmpty(telNum))
            {
                sql += "and  c_Num='" + telNum + "'";
            }
            var list = dal.SqlQuery<CustomerNew>(sql);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 会员详细资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// h_CustomerNew 会员资料表
        /// c_num 会员编号（手机号码）
        /// c_Name 会员名称
        /// c_sex 性别
        /// c_address 会员地址
        /// c_birthday 会员生日
        /// c_identity 会员ID
        [HttpGet, Route("GetCustomerNewBySql")]
        public string GetCustomerNewBySql(string vipNum)
        {
            var sql = @"select c_num,c_Name,c_sex,c_address,c_birthday,c_identity from h_CustomerNew where 1=1";
            if (!string.IsNullOrEmpty(vipNum))
            {
                sql += "and c_Num='" + vipNum + "'";
            }
            var list = dal.SqlQuery<CustomerNewSelect>(sql);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 销售表
        /// </summary>
        /// <param name="vipid"></param>
        /// <returns></returns>
        /// h_chtj 销售表
        /// c_chdh 销售单号
        /// c_ShopName 门店名称
        /// c_VipId 会员ID
        /// c_Total 销售总价
        /// c_TotalVip  折后价格
        /// c_Date  时间
        [HttpGet, Route("GetOrderBySql")]
        public string GetOrderBySql(string vipNo)
        {
            var sql = @"select c_chdh,c_ShopName,c_VipId,c_Total,c_TotalVip,c_Date from h_chtj where";
            if (!string.IsNullOrEmpty(vipNo))
            {
                sql += " c_VIPID=" + vipNo;
            }
            var list = dal.SqlQuery<h_chtjWX>(sql);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 销售详情
        /// </summary>
        /// <param name="orderDetail">orderDetail提供销售单号</param>
        /// <returns></returns>
        /// h_ch 销售详细表
        /// c_chdh  订单编号
        /// c_chxh 序号
        /// c_name 商品名称
        /// c_num 商品编号
        /// c_Cprice 销售价格（吊牌价格）
        /// c_CpriceVIP 折后价格
        /// c_DisCount 折扣
        /// c_Quantity 销售数量
        [HttpGet, Route("GetOrderDetail")]
        public string GetOrderDetail(string orderNo)
        {
            var sql = @"select c_chdh,c_chxh,c_name,c_num,c_cprice,c_cpriceVIP,c_DisCount,c_quantity from h_ch where";
            if (!string.IsNullOrEmpty(orderNo))
            {
                sql += " c_chdh='" + orderNo + "'";
            }
            var list = dal.SqlQuery<h_chWX>(sql);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 积分表
        /// </summary>
        /// <param name="vipid"></param>
        /// <returns></returns>
        /// h_IntegralIn  积分表
        /// i_chdh  积分单号
        /// i_VIPID  会员ID唯一标识
        /// i_Integral 会员积分
        /// i_Date  日期       
        [HttpGet, Route("GetIntegralInBySql")]
        public string GetIntegralInBySql(string vipid)
        {
            var sql = @"select i_chdh,i_VipID,i_Integral,i_Date from h_IntegralIn where";
            if (!string.IsNullOrEmpty(vipid))
            {
                sql += " i_VipID=" + vipid;
            }
            var list = dal.SqlQuery<IntegralInWX>(sql);
            return JsonConvert.SerializeObject(list);
        }

    */

        #endregion

        /// <summary>
        /// 订单生成通知
        /// </summary>
        /// openid
        /// 订单号
        /// 消费总额
        /// 数量
        /// 原价
        /// 折后价总价
        /// 门店名称
        /// 下单时间
        /// <param name="model"></param>
        [HttpPost, Route("SendOrderNotice")]
        public bool SendOrderNotice([FromBody] OrderInfo info)
        {
            try
            {
                string shop_appid = WeixinConfig.AppID;//公众账号ID
                //string access_token = WeixinConfig.TokenHelper.GetToken();
                string access_token = AccessTokenContainer.TryGetAccessToken(WeixinConfig.AppID, WeixinConfig.AppSecret, true);

                string openid = info.openid;
                string template_id = System.Configuration.ConfigurationManager.AppSettings["SpollingSendPlaceOrderNotice"];//模板ID  正式号模板ID
                var domain = System.Configuration.ConfigurationManager.AppSettings["Domain"];
                string redirect_uri = "http://" + domain + "/weixin/seeorderdetail?orderid=" + info.orderId;
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}";
                url = string.Format(url, shop_appid, HttpUtility.UrlEncode(redirect_uri), "wanlixin");

                string first = "尊敬的会员，您的订单已支付成功！";
                string keyword1 = info.shopName.ToString();//门店名称
                string keyword2 = info.orderId.ToString();//订单编号
                string keyword3 = info.num.ToString();//订单数量
                string keyword4 = info.price.ToString();//销售金额

                string remark = "感谢您的信任，欢迎您再次光临！";

                Tuple<string, string, Color>[] data = new Tuple<string, string, Color>[]{
                    new Tuple<string,string,Color>("first",first,Color.Black),
                    new Tuple<string,string,Color>("keyword1",keyword1,Color.Black),
                    new Tuple<string,string,Color>("keyword2",keyword2,Color.Black),
                    new Tuple<string,string,Color>("keyword3",keyword3,Color.Black),
                    new Tuple<string,string,Color>("keyword4",keyword4,Color.Black),
                    new Tuple<string,string,Color>("remark",remark,Color.Black)
                    };

                IDictionary<string, object> dataDict = new Dictionary<string, object>();
                foreach (Tuple<string, string, Color> item in data)
                {
                    dataDict.Add(item.Item1, new { value = item.Item2, color = GetColorString(item.Item3) });
                }

                SendTemplateMessageResult result = TemplateApi.SendTemplateMessage(access_token, openid, template_id, url, dataDict);

                Tools.MessBox(result.errcode + "|" + result.errmsg, "//Log//SendOrderNotice//return");
                if (result.errmsg == "ok")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString(), "//Log//SendOrderNotice");
                return false;
            }
        }

        /// <summary>
        /// 将颜色额转换成由16进制RGB分量表示的字符串。
        /// 例如：红色-#FF0000
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>返回颜色字符串</returns>
        public static string GetColorString(Color color)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
        }
    }
}
