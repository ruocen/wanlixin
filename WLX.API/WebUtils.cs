using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLX.Utils;

namespace WLX.API
{
    public class WebUtils
    {
        public static bool Send<T>(string url, T t)
        {
            var body = new ApiBody();
            body.SendType = SendType.Post;
            body.ApiUrl = url;
            body.SendObj = t;
            int totalCount = 0;
            var result = ApiHelper.GetApiData(body, ref totalCount);
            return result;
        }
    }
}