using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
using WLX.Data.Filter;
using WLX.Utils;

namespace WLX.WeiXin
{
    public class WebUtils
    {
        /// <summary>
        /// 通过唯一编号获得数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Get<T>(string url, string id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("telNum", id);

            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData<T>(body, ref totalCount);
        }
        public static T Get2<T>(string url, string id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("vipNum", id);

            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData<T>(body, ref totalCount);
        }
        /// <summary>
        /// 通过某个字段获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Get<T>(string url, string name, string value)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(name, value);

            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData<T>(body, ref totalCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Get<T>(string url, IDictionary<string, object> parameters)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData<T>(body, ref totalCount);
        }

        /// <summary>
        /// 通过过滤条件获取一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static T Get<T>(string url, BaseFilter filter)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.SendObj = filter;
            int totalCount = 0;
            var result = ApiHelper.GetApiData<T>(body, ref totalCount);
            return result;
        }
        public static bool Save<T>(string url, T t)
        {
            var body = new ApiBody();
            body.SendType = SendType.Post;
            body.ApiUrl = url;
            body.SendObj = t;
            int totalCount = 0;
            var result = ApiHelper.GetApiData(body, ref totalCount);
            return result;
        }
        //public static bool Save<T>(string url, T t, MES_T_USER user, ref string msg) where T : MesEntityBase
        //{
        //    var obj = BuildUpdateEntity(t, user);
        //    var body = new ApiBody();
        //    body.SendType = SendType.Post;
        //    body.ApiUrl = url;
        //    body.SendObj = obj;
        //    int totalCount = 0;
        //    var result = ApiHelper.GetApiData(body, ref totalCount, ref msg);
        //    return result;
        //}
        /// <summary>
        /// 执行某个接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool Do(string url, BaseFilter filter)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.SendObj = filter;
            int totalCount = 0;
            var result = ApiHelper.GetApiData(body, ref totalCount);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(string url, string id)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            body.Params = parameters;
            parameters.Add("id", id);
            int totalCount = 0;
            var result = ApiHelper.GetApiData(body, ref totalCount);
            return result;
        }
        public static bool BatchDelete<T>(string url, string[] id)
        {
            var body = new ApiBody();
            var sendStr = string.Join(",", id);
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ids", sendStr);
            body.Params = parameters;
            int totalCount = 0;
            var result = ApiHelper.GetApiData(body, ref totalCount);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> List<T>(string url, IDictionary<string, object> parameters, ref int totalCount)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            var result = ApiHelper.GetApiData<List<T>>(body, ref totalCount);
            return result;
        }
        public static List<T> List<T>(string url, BaseFilter filter, ref int totalCount)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.SendObj = filter;
            var result = ApiHelper.GetApiData<List<T>>(body, ref totalCount);
            return result;
        }
        public static PagedList<T> PageList<T>(string url, BasePageFilter filter)
        {
            var startIndex = filter.Start / filter.Limit + 1;
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.SendObj = filter;
            int totalCount = 0;
            var result = ApiHelper.GetApiData<List<T>>(body, ref totalCount);
            PagedList<T> pageList = null;
            if (result != null)
                pageList = new PagedList<T>(result, startIndex, filter.Limit, totalCount);
            return pageList;
        }

        public static IDictionary<string, object> PageParameter(int pageIndex)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("pageIndex", pageIndex);
            parameters.Add("pageSize", DataUtils.PageSize);
            return parameters;
        }

        public static IDictionary<string, object> DefaultParameter(string cid)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("cid", cid);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool Exits<T>(string url, IDictionary<string, object> parameters)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            T t = ApiHelper.GetApiData<T>(body, ref totalCount);
            if (t == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 通过唯一编号获得数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Post(string url, IDictionary<string, object> parameters)
        {
            var body = new ApiBody();
            body.SendType = SendType.Post;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData(body, ref totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool Execute(string url, IDictionary<string, object> parameters)
        {
            var body = new ApiBody();
            body.SendType = SendType.Get;
            body.ApiUrl = url;
            body.Params = parameters;
            int totalCount = 0;
            return ApiHelper.GetApiData(body, ref totalCount);

        }
    }
}