using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Utils.WebUtils
{
    public class CommonUtil
    {
        public static string GetNewID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string Urlparam2Json(string param)
        {
            if (string.IsNullOrEmpty(param)) return string.Empty;
            var list = param.Split('&');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var kv = item.Split('=');
                dic.Add(kv[0], kv[1]);
            }
            string json = JsonConvert.SerializeObject(dic);
            return json;
        }
        public static string Json2Urlparam(string json)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var sb = new StringBuilder();
            foreach (var sA in obj)//参数名ASCII码从小到大排序（字典序）；
            {
                if (string.IsNullOrEmpty(sA.Value)) continue;//参数的值为空不参与签名；
                sb.Append(sA.Key).Append("=").Append(sA.Value).Append("&");
            }
            var param = sb.ToString().TrimEnd('&');
            return param;
        }
    }
}
