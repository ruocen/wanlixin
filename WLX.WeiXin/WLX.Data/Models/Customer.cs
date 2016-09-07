using System;
using System.ComponentModel.DataAnnotations;

namespace WLX.Data.Models
{
    public class Customer
    {
        [Key]
        [MaxLength(32)]
        public string ID { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Sex { get; set; }
        [MaxLength(50)]
        public string OpenId { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Province { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string BabySituation { get; set; }

        public DateTime? BabyBirthday { get; set; }
        public DateTime? CreateDate { get; set; }
        [MaxLength(200)]
        public string Remark { get; set; }
        public bool IsApproved { get; set; }
        public int LamaLevel { get; set; }
        [MaxLength(500)]
        public string PicUrl { get; set; }
        [MaxLength(50)]
        public string WeixinID { get; set; }
        public int MobileIsCheck { get; set; }
    }
}
