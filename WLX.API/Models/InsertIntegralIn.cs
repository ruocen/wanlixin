using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.API.Models
{
    public class InsertIntegralIn
    {
        public string i_VIPId { get; set; }
        public string i_vipnum { get; set; }
        public Decimal i_Integral { get; set; }
        public string i_ShopId { get; set; }
        public string i_Memo { get; set; }

        public DateTime i_Date { get; set; }
    }
}