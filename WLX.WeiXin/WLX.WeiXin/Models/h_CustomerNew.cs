using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    /// <summary>
    /// 会员详细资料
    /// </summary>
    public class h_CustomerNew
    {
        /// <summary>
        /// 会员编号（手机号码）
        /// </summary>
        public string c_num { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string c_Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string c_sex { get; set; }
        /// <summary>
        /// 会员地址
        /// </summary>
        public string c_address { get; set; }
        /// <summary>
        /// 会员生日
        /// </summary>
        public DateTime c_BirthDay { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int c_Identity { get; set; }
    }
}