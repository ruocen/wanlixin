using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.API.Models
{
    public class h_chtjWX
    {//c_chdh,c_chxh,c_name,c_num,c_cprice,c_cpriceVIP,c_DisCount,c_quantity
        public string c_chdh { get; set; }
        public int c_quantity { get; set; }
        public string c_shopname { get; set; }
        public int v_vipid { get; set; }
        public decimal c_Total { get; set; }
        public decimal c_TotalVip { get; set; }
        public DateTime c_Date { get; set; }
       // c_chdh,c_ShopName,c_VipId,c_Total,c_TotalVip,c_Date
    }
}