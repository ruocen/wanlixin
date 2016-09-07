using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    /// <summary>
    /// 积分表
    /// </summary>
    public class h_IntegralIn
    {
        /// <summary>
        /// 积分单号
        /// </summary>
        public string i_chdh { get; set; }
        /// <summary>
        /// 会员ID唯一标识
        /// </summary>
        public string i_VipID { get; set; }
        /// <summary>
        /// 会员积分
        /// </summary>
        public string i_Integral { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string i_Date { get; set; }
    }
}