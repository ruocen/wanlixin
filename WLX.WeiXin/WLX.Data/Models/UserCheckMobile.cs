using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Data.Models
{
    public class UserCheckMobile
    {
        [Key]
        [MaxLength(32)]
        public string ID { get; set; }
        [MaxLength(32)]
        public string UserID { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        public DateTime UpdateTime { get; set; }
        [MaxLength(20)]
        public string CheckCode { get; set; }
        public int GetCodeTimes { get; set; }

    }
}
