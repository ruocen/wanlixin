using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using WLX.Data;
using WLX.Data.DataBase;
using WLX.Data.Models;
using WLX.Utils.WebUtils;
using WLX.API.Models;

namespace WLX.API.Controllers
{
    public class SaledddController : ApiController
    {
        BaseOption<h_ch> bs = new BaseOption<h_ch>();
        CommDAL dal = new CommDAL();

        public string GetSaleListBySql(string id)
        {
            var sql = @"select * from h_ch where";
            if (!string.IsNullOrEmpty(id))
            {
                sql += " (c_ID='" + id + "')";
            }
            var list = dal.SqlQuery<h_ch>(sql);
            return JsonConvert.SerializeObject(list);
        }
    }
}
