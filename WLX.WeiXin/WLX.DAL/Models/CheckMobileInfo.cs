using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.DAL.Models
{
    public class CheckMobileInfo
    {
        public string CheckID { get; set; }
        public string UserID { get; set; }
        public string Mobile { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CheckCode { get; set; }
        public int GetCodeTimes { get; set; }
    }
}
