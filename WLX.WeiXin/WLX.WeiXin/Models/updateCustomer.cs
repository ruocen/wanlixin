using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    /// <summary>
    /// 更新会员的openid
    /// </summary>
    public class updateCustomer
    {
        public string vpid { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string address { get; set; }
        public DateTime birthday { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string wx_name { get; set; }
        public string num { get; set; }

    }
}