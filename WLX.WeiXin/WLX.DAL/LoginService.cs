using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLX.Data;
using WLX.Data.Models;
using WLX.Utils;
using WLX.Utils.WebUtils;

namespace WLX.DAL
{
    public class LoginService
    {
        public Customer CustomerLogin(string Name, string OpenId, string Phone, string Comment, string PicUrl, int Sex, string Province, string City)
        {
            Customer customer;
            try
            {
                using (EntityContext db = new EntityContext())
                {
                     customer = db.Customers.FirstOrDefault(o => o.OpenId == OpenId);
                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.ID = CommonUtil.GetNewID();
                        customer.Name = Name;
                        customer.OpenId = OpenId;
                        customer.Phone = Phone;
                        customer.CreateDate = DateTime.Now;
                        customer.Remark = Comment;
                        customer.IsApproved = false;
                        customer.LamaLevel = 0;
                        customer.PicUrl = PicUrl;
                        if (Sex.Equals(1))
                            customer.Sex = "男";
                        else if (Sex.Equals(2))
                            customer.Sex = "女";
                        else
                            customer.Sex = "未知";
                        customer.Province = Province;
                        customer.City = City;
                        db.Customers.Add(customer);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString());
                customer = null;
            }
            //if (RedisOnline)
            //{
            //    PreloadService.LoadLamaInfo();
            //    Tools.MessBox("Lama缓存更新完成");
            //}
            return customer;
        }

    }
}
