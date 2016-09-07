using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    /// <summary>
    /// 销售表
    /// </summary>
    public class h_chtj
    {
        /// <summary>
        /// 销售单号
        /// </summary>
        public string c_chdh { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string c_ShopName { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public string c_VipId { get; set; }
        /// <summary>
        /// 销售总价
        /// </summary>
        public string c_Total { get; set; }
        /// <summary>
        /// 折后价格
        /// </summary>
        public string c_TotalVip { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string c_Date { get; set; }
    }
}