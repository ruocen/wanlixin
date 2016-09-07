using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLX.WeiXin.Models
{
    public class CustomerNewSelect
    {
        public string c_num { get; set; }
        public string c_Name { get; set; }
        public string c_sex { get; set; }
        public string c_address { get; set; }
        public DateTime c_BirthDay { get; set; }
        public int c_Identity { get; set; }
    }
}