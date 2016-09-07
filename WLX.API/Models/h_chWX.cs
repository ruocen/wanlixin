using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.API.Models
{
    public class h_chWX
    {//c_chdh,c_chxh,c_name,c_num,c_cprice,c_cpriceVIP,c_DisCount,c_quantity,c_color,c_size 
        public string c_chdh { get; set; }
        public int c_chxh { get; set; }
        public string c_name { get; set; }
        public string c_num { get; set; }
        public string c_color { get; set; }
        public string c_size { get; set; }
        public decimal c_cprice { get; set; }
        public decimal c_cpriceVIP { get; set; }
        public decimal c_DisCount { get; set; }
        public int c_quantity { get; set; }
    }
}