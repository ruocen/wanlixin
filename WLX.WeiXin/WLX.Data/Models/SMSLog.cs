using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Data.Models
{
    public class SMSLog
    {
        [Key]
        [MaxLength(32)]
        public string ID { get; set; }
        [MaxLength(32)]
        public string UserId { get; set; }
        [MaxLength(50)]
        public string OpenId { get; set; }
        [MaxLength(50)]
        public string Message { get; set; }
        public int? SMSTYPE { get; set; }
        [MaxLength(1000)]
        public string ReturnStr { get; set; }
        public DateTime? SendTime { get; set; }
        [MaxLength(50)]
        public string MobilePhone { get; set; }       
        public bool IsSuccess { get; set; }
    }
}
