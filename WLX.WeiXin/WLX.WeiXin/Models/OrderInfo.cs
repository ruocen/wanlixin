using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    public class OrderInfo
    {
        /// <summary>
        /// 订单号 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 折后总价
        /// </summary>
        public decimal vipPrice { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime orderTime { get; set; }
        public string openid { get; set; }
    }
}