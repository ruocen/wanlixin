using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLX.Data;
using WLX.Data.Models;

namespace WLX.DAL
{
    public class UserService
    {
        CommDAL dal = new CommDAL();
        /// <summary>
        /// 通过验证过的手机取得对应的用户信息，手机没通过验证则发返回null
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static Customer GetUserInfoByCheckdMobile(string mobile)
        {
            using (EntityContext db = new EntityContext())
            {
                return db.Customers.Where(s => s.MobileIsCheck > 0 && s.Phone == mobile).FirstOrDefault();
            }
        }
    }
}
