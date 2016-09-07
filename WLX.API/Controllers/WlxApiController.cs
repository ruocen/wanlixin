using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using WLX.Data;
using WLX.Data.DataBase;
using WLX.Data.Models;
using WLX.Utils.WebUtils;
using System.Data.SqlClient;
using WLX.API.Models;
using System.Data;
using WLX.Utils;

namespace WLX.API.Controllers
{
    [RoutePrefix("api/WlxApi")]
    public class WlxApiController : ApiController
    {
        //http://localhost:14639/api/sale/?id=5002740&name=T恤

        CommDAL dal = new CommDAL();

        #region
        /// <summary>
        /// 会员绑定,根据手机号码绑定
        /// </summary>
        /// <param name="telNum"></param>
        /// <returns></returns>
        /// h_CustomerNew会员资料表
        /// c_num 会员编号
        /// c_identity 会员ID（唯一标识）
        [HttpGet, Route("GetCustomerBySql")]
        public CustomerNew GetCustomerBySql(string telNum)
        {
            var sql = @"select c_num,c_identity from h_CustomerNew where 1=1";
            if (!string.IsNullOrEmpty(telNum))
            {
                sql += "and  c_Num='" + telNum + "'";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(CommandType.Text, sql);
            CustomerNew info = EntityObject.DT2SimpleEntity<CustomerNew>(ds.Tables[0]);
            return info;
        }
        /// <summary>
        /// 会员详细资料
        /// </summary>
        /// <param name="vipNum">111</param>
        /// <returns></returns>
        /// h_CustomerNew 会员资料表
        /// c_num 会员编号（手机号码）
        /// c_Name 会员名称
        /// c_sex 性别
        /// c_address 会员地址
        /// c_birthday 会员生日
        /// c_identity 会员ID
        /// c_QQ  会员QQ
        /// c_Mail  会员邮箱
        /// c_Company  所在单位
        [HttpGet, Route("GetCustomerNewBySql")]
        public CustomerNewSelect GetCustomerNewBySql(string vipNum)
        {
             var sql = @"select c_num,c_Name,c_sex,c_address,c_birthday,c_identity,c_DisCount,c_QQ,c_Mail,c_Company from h_CustomerNew where 1=1";
            if (!string.IsNullOrEmpty(vipNum))
            {
                sql += "and c_Num='" + vipNum + "'or c_openid='" + vipNum + "'";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(CommandType.Text, sql);
            CustomerNewSelect info = EntityObject.DT2SimpleEntity<CustomerNewSelect>(ds.Tables[0]);
            return info;
        }
        /// <summary>
        /// 销售表
        /// </summary>
        /// <param name="vipNo"></param>
        /// <returns></returns>
        /// h_chtj 销售表
        /// c_chdh 销售单号
        /// c_ShopName 门店名称
        /// c_VipId 会员ID
        /// c_Total 销售总价
        /// c_TotalVip  折后价格
        /// c_Date  时间
        /// c_quantity 数量
        [HttpGet, Route("GetOrderBySql")]
        public string GetOrderBySql(string vipNo)
        {
            var sql = @"select c_chdh,c_quantity,c_ShopName,c_VipId,c_Total,c_TotalVip,c_Date from h_chtj where";
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
        /// <param name="orderNo">orderDetail提供销售单号</param>
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
        /// c_color   颜色  
        /// c_size   尺寸
        [HttpGet, Route("GetOrderDetail")]
        public string GetOrderDetail(string orderNo)
        {
            var sql = @"select c_chdh,c_chxh,c_name,c_num,c_color,c_size,c_cprice,c_cpriceVIP,c_DisCount,c_quantity from h_ch where";
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
        /// <param name="vipId"></param>
        /// <returns></returns>
        /// h_IntegralIn  积分表
        /// i_chdh  积分单号
        /// i_VIPID  会员ID唯一标识
        /// i_Integral 会员积分
        /// i_Date  日期       
        [HttpGet, Route("GetIntegralInBySql")]
        public string GetIntegralInBySql(string vipId)
        {
            var sql = @"select i_chdh,i_VipID,i_Integral,i_Date from h_IntegralIn where";
            if (!string.IsNullOrEmpty(vipId))
            {
                sql += " i_VipID=" + vipId;
            }
            var list = dal.SqlQuery<IntegralInWX>(sql);
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 总积分
        /// </summary>
        /// <param name="vipId">会员编号</param>
        /// <returns></returns>
        [HttpGet, Route("GetIntegralSumBySql")]
        public string GetIntegralSumBySql(string vipId)
        {
            var sql = @"select sum(i_Integral) as total, c_DIsCount as DisCount from h_IntegralIn with(nolock) inner join h_CustomerNew with(nolock)
 on (c_identity = '" + vipId + "' and c_identity = i_VipID ) where ";
            if (!string.IsNullOrEmpty(vipId))
            {
                sql += "i_vipid='" + vipId + "' or i_VipID in (select  c_oldid from h_CustomerNew with(nolock) where c_identity ='" + vipId + "' )group by c_DIsCount";
            }
            var list = dal.SqlQuery<IntegralSum>(sql);
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 会员更新
        /// </summary>
        /// <param name = "vpid" >会员ID</param >
        /// <param name="name">会员名称</param>
        /// <param name = "sex" >会员性别</param >
        /// <param name="address">地址</param>
        /// <param name = "birthday" >会员生日</param >
        /// <param name = "openid" >微信Openid</param >
        /// <param name = "wx_name" >微信名称</param >
        /// <param name = "num" >手机号码</param >
        /// <returns ></returns >
        [HttpPost, Route("UpdateVip")]
        public int UpdateVip([FromBody]updateCustomernew model)
        {
            SqlHelper helper = new SqlHelper();
            SqlParameter[] prams = new SqlParameter[3];
            prams[0] = new SqlParameter("@c_vipid", model.vipid);
            prams[1] = new SqlParameter("@c_openid", model.openid);
            prams[2] = new SqlParameter("@C_wxname", model.wx_name);
            //prams[1] = new SqlParameter("@c_Name", model.name);
            //prams[2] = new SqlParameter("@c_Sex", model.sex);
            //prams[3] = new SqlParameter("@c_address", model.address);
            //prams[4] = new SqlParameter("@c_birthday", model.birthday);
            //prams[7] = new SqlParameter("@c_num", model.Num);
            //var count = helper.ExecuteNonQuery(@"update  dbo.h_customernew set c_num=@c_num,c_Name=@c_Name,c_Sex=@c_Sex,c_address=@c_address,c_birthday=@c_birthday,c_openid=@c_openid,c_wxname=@c_wxname where c_identity=@c_vipid", prams);
            var count = helper.ExecuteNonQuery(@"update  dbo.h_customernew set c_openid=@c_openid,c_wxname=@c_wxname where c_identity=@c_vipid", prams);
            return count; //返回影响的行数
        }


        /// <summary>
        /// 会员更改信息
        /// </summary>
        /// <param name = "vpid" >会员ID</param >
        /// <param name="name">会员名称</param>
        /// <param name = "sex" >会员性别</param >
        /// <param name="address">地址</param>
        /// <param name = "birthday" >会员生日</param >
        /// <param name = "openid" >微信Openid</param >
        /// <param name = "wx_name" >微信名称</param >
        /// <param name = "num" >手机号码</param >
        /// <returns ></returns >
        [HttpPost, Route("UpdateCustomernew")]
        public int UpdateCustomernew([FromBody]updateCustomernew model)
        {
            SqlHelper helper = new SqlHelper();
            SqlParameter[] prams = new SqlParameter[8];
            prams[0] = new SqlParameter("@c_vipid", model.vipid);
            prams[1] = new SqlParameter("@c_Name", model.name);
            prams[2] = new SqlParameter("@c_Sex", model.sex);
            prams[3] = new SqlParameter("@c_address", model.address);
            prams[4] = new SqlParameter("@c_birthday", model.birthday);
            prams[5] = new SqlParameter("@c_QQ", model.QQ);
            prams[6] = new SqlParameter("@c_Mail", model.Mail);
            prams[7] = new SqlParameter("@c_Company", model.Company);
            var count = helper.ExecuteNonQuery(@"update  dbo.h_customernew set c_Name=@c_Name,c_Sex=@c_Sex,c_address=@c_address,c_birthday=@c_birthday,c_QQ=@c_QQ,c_Mail=@c_Mail,c_Company=@c_Company where c_identity=@c_vipid", prams);
            return count; //返回影响的行数
        }

        /// <summary>
        /// 注册申请新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost, Route("InsertVip")]
        public int InsertVip([FromBody]InsertCustomerNew model)
        {
            SqlHelper helper = new SqlHelper();
            SqlParameter[] prams = new SqlParameter[3];
            prams[0] = new SqlParameter("@c_num", model.num);
            prams[1] = new SqlParameter("@c_openid", model.openid);
            prams[2] = new SqlParameter("@c_wxname", model.wxname);

            var count = helper.ExecuteNonQuery(@"insert into   dbo.h_CustomerNew(c_num,c_discount,c_bookindate,c_openid,c_wxname,c_memo2)values(@c_num,'1.00',convert(varchar(20),getdate(),120),@c_openid,@c_wxname,'微信线上注册')", prams);
            return count; //返回影响的行数
        }
        /// <summary>
        /// 通过订单编号推送订单消息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>

        [HttpGet, Route("sendOrder")]
        public bool sendOrder(string orderId)
        {
            SqlHelper helper = new SqlHelper();

            string sql = @"select  c_chdh as orderId, a.c_total as price, c_totalVIP as vipPrice, c_quantity as num, c_ShopName as shopName, c_date as orderTime, c_openid as openid from h_chtj as a 
                           inner join h_customerNew as b on a.c_vipId = b.c_identity
                           where a.c_chdh='" + orderId + "'";
            DataSet ds = SqlHelper.ExecuteDataSet(CommandType.Text, sql);
            OrderInfo info = EntityObject.DT2SimpleEntity<OrderInfo>(ds.Tables[0]);
            if (true)
            {

            }
            var result = WebUtils.Send<OrderInfo>("http://www.xmwanlixin.cn/api/WlxApi/SendOrderNotice", info);
            if (result)
            {
                SqlParameter[] prams = new SqlParameter[1];
                prams[0] = new SqlParameter("@c_chdh", orderId);
                var count = helper.ExecuteNonQuery(@"update dbo.h_chtj set c_wxts='0' where c_chdh=@c_chdh", prams);
            }
            return result;
        }


        #endregion
    }
}
