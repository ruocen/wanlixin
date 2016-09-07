using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.API.Models
{
    public class sendOrder
    {
        public string orderId { get; set; }
        //public string totalPrice { get; set; }
        public int num { get; set; }
        public decimal price  {get;set;}
        public decimal vipPrice {get;set;}
        public string shopName {get;set;}
        public DateTime orderTime {get;set;}
        public string openId { get; set; }
     
    }
}