using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WLX.Data;
using WLX.Data.DataBase;
using WLX.Data.Models;
using WLX.Utils;
using WLX.Utils.WebUtils;

namespace WLX.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/City")]
    public class CityController:ApiController
    {
        BaseOption<City> bs = new BaseOption<City>();
        CommDAL dal = new CommDAL();

        #region API 实例
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("GetCityList")]
        //public string GetCityList(string keyword)
        //{
        //    Expression<Func<City, bool>> where = s => s.CityID > 0;
        //    if (!string.IsNullOrEmpty(keyword))
        //        where = where.And(s => s.CityNo.Contains(keyword) || s.CityName.Contains(keyword));
        //    var list = dal.GetList(where);
        //    return JsonConvert.SerializeObject(list);
        //}
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetCityListBySql")]
        public string GetCityListBySql(string keyword)
        {
            string sql = @"select * from City where 1=1";
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " and CityNo like '%" + keyword + "%' or CityName like '%" + keyword + "%' or ProvinceNo like '%" + keyword + "%'";
            }
            var list = dal.SqlQuery<City>(sql);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 存储过程方式获取
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        [HttpGet, Route("GetCityListByProcedure")]
        public string GetCityListByProcedure(string Condition)
        {
            try
            {
                SqlParameter[] prams = new SqlParameter[1];
                prams[0] = new SqlParameter("@Condition", Condition);
                DataSet ds = SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "GetCityList", prams);
                if (ds != null)
                    return JsonConvert.SerializeObject(ListUtils.ToList<City>(ds.Tables[0]));
                else
                    return null;
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString());
                return null;
            }

        }

        [HttpPost, Route("SaveCity")]
        public int SaveCity()
        {
            SqlHelper helper = new SqlHelper();
            City model = new City();
            model.CityName = "龙岩";
            model.CityNo = "ly001";
            model.ProvinceNo = "FJ001";
            //bs.Add(model);

            SqlParameter[] prams = new SqlParameter[3];
            prams[0] = new SqlParameter("@CityName", model.CityName);
            prams[1] = new SqlParameter("@CityNo", model.CityNo);
            prams[2] = new SqlParameter("@ProvinceNo", model.ProvinceNo);

            var count = helper.ExecuteNonQuery(@"insert into dbo.city(CityName,CityNo,ProvinceNo) values(@CityName,@CityNo,@ProvinceNo)", prams);
            return count;
        }


        /// <summary>
        /// 添加城市
        /// </summary>
        [HttpPost, Route("AddCity")]
        public City AddCity(City city)
        {
            return dal.SaveOrUpdate(city);
        }
        //修改
        [HttpPost, Route("EditCity")]
        public City EditCity(City city)
        {
            return dal.SaveOrUpdate(city);
        }
        //删除
        [HttpPost, Route("DeleteCity")]
        public bool DeleteCity(string id)
        {
            return dal.Delete<City>(id);
        }
        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        [HttpGet, Route("GetCity")]
        public void GetCity(string id)
        {
            var model = dal.FindById<City>(id);
        }
        ///// <summary>
        ///// sql查询列表
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpGet, Route("GetCityListBySql")]
        //public List<City> GetCityListBySql(City model)
        //{
        //    string Sql = @"  SELECT  * FROM    City c
        //                        WHERE c.CityID=@CityID";
        //    SqlParameter parameter = new SqlParameter("@CityID", model.CityID);
        //    DataTable dt = SqlHelper.ExecuteDataTable(Sql, parameter);
        //    if (dt == null || dt.Rows.Count == 0) return null;
        //    var list = Utils.WebUtils.EntityObject.DT2EntityList<City>(dt);
        //    return list;
        //}
        #endregion
    }
}