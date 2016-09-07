using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    /// <summary>
    /// 销售详情表
    /// </summary>
    public class h_ch
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string c_chdh { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string c_chxh { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string c_name { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string c_num { get; set; }
        /// <summary>
        /// 销售价格（吊牌价格）
        /// </summary>
        public string c_cprice { get; set; }
        /// <summary>
        /// 折后价格
        /// </summary>
        public string c_cpriceVIP { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string c_DisCount { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public string c_quantity { get; set; }
    }
}