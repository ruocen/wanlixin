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

namespace WLX.API.Controllers
{
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiController
    {
        //SqlHelper helper = new SqlHelper();

        //http://localhost:14639/api/sale/?id=5002740&name=T恤
        [HttpGet,Route("GetSaleListBySql")]
        public string GetSaleListBySql(string aaa)
        {
            BaseOption<h_ch> bs = new BaseOption<h_ch>();
            CommDAL dal = new CommDAL();
            var sql = @"select c_chdh,c_name,c_num  from h_ch where";
            if (!string.IsNullOrEmpty(aaa))
            {
                sql += " c_VIPID=" + aaa ;
            }
            var list = dal.SqlQuery<h_ch>(sql);
            return JsonConvert.SerializeObject(list);
        }
    }
}
